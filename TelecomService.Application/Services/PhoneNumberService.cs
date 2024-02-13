using Microsoft.Extensions.Logging;
using TelecomService.Application.Interfaces;
using TelecomService.Application.Models;
using TelecomService.Core.Entities;

namespace TelecomService.Application.Services;

public class PhoneNumberService : IPhoneNumberService
{
    private readonly IPhoneNumberRepository _phoneNumberRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<PhoneNumberService> _logger;

    public PhoneNumberService(ILogger<PhoneNumberService> logger, 
        IPhoneNumberRepository phoneNumberRepository, 
        ICustomerRepository customerRepository)
    {
        _phoneNumberRepository = phoneNumberRepository ?? throw new ArgumentNullException(nameof(phoneNumberRepository));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<PhoneNumberDto>> GetAllPhoneNumbersAsync()
    {
        _logger.LogInformation("[Service {Service}, Method {Method}]: Get all phone number",
            nameof(PhoneNumberService),
            nameof(GetAllPhoneNumbersAsync));

        var customers = await _customerRepository.GetAllCustomersAsync();
        var phoneNumbers = await _phoneNumberRepository.GetAllPhoneNumbersAsync();
        var result = phoneNumbers.Join(
            customers,
            phoneNumber => phoneNumber.CustomerId,
            customer => customer.Id,
            (phoneNumber, customer) => new PhoneNumberDto
            {
                Id = phoneNumber.Id,
                PhoneNumber = phoneNumber.PhoneNumberValue,
                Active = phoneNumber.Active,
                CustomerId = phoneNumber.CustomerId,
                CustomerName = customer.Name
            });

        return result;
    }

    public async Task<PhoneNumberDto?> GetPhoneNumberByIdAsync(string phoneNumberId)
    {
        _logger.LogInformation("[Service {Service}, Method {Method}]: Get phone number by id: {phoneNumberId}",
            nameof(PhoneNumberService),
            nameof(GetPhoneNumberByIdAsync),
            phoneNumberId);

        var phoneNumber = await _phoneNumberRepository.GetPhoneNumberByIdAsync(phoneNumberId);
        return phoneNumber != null? new PhoneNumberDto
        {
            Id = phoneNumber.Id,
            PhoneNumber = phoneNumber.PhoneNumberValue,
            Active = phoneNumber.Active,
            CustomerId = phoneNumber.CustomerId,
        } : null;
    }

    public async Task<IEnumerable<PhoneNumberDto>> GetPhoneNumbersByCustomerIdAsync(string customerId)
    {
        _logger.LogInformation("[Service {Service}, Method {Method}]: Get all phone numbers by customer id: {customerId}",
            nameof(PhoneNumberService),
            nameof(GetPhoneNumbersByCustomerIdAsync),
            customerId);

        var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
        var phoneNumbers = await _phoneNumberRepository.GetPhoneNumbersByCustomerIdAsync(customerId);
        var result = phoneNumbers.Select(phoneNumber => new PhoneNumberDto
        {
            Id = phoneNumber.Id,
            PhoneNumber = phoneNumber.PhoneNumberValue,
            Active = phoneNumber.Active,
            CustomerId = phoneNumber.CustomerId,
            CustomerName = customer?.Name
        });

        return result;
    }
    
    public async Task<PhoneNumberDto> AddPhoneNumberAsync(PhoneNumberDto newPhoneNumberDto)
    {
        _logger.LogInformation("[Service {Service}, Method {Method}]: Add new phone number",
            nameof(PhoneNumberService),
            nameof(AddPhoneNumberAsync));

        PhoneNumber newPhoneNumber = new PhoneNumber
        {
            Id = Guid.NewGuid().ToString(),
            PhoneNumberValue = newPhoneNumberDto.PhoneNumber,
            CustomerId = newPhoneNumberDto.CustomerId,
            Active = false
        };

        var addedPhoneNumber = await _phoneNumberRepository.AddPhoneNumberAsync(newPhoneNumber);
        newPhoneNumberDto.Id = addedPhoneNumber.Id;
        return newPhoneNumberDto;
    }

    public async Task<PhoneNumberDto?> ActivatePhoneNumberAsync(string phoneNumberId)
    {
        _logger.LogInformation("[Service {Service}, Method {Method}]: Activate phone number: {phoneNumberId}",
            nameof(PhoneNumberService),
            nameof(ActivatePhoneNumberAsync),
        phoneNumberId);

        PhoneNumber? phoneNumber = await _phoneNumberRepository.GetPhoneNumberByIdAsync(phoneNumberId);
        if (phoneNumber != null)
        {
            phoneNumber.Active = true;
            var activatedPhoneNumber = await _phoneNumberRepository.ActivatePhoneNumberAsync(phoneNumber);
            return new PhoneNumberDto
            {
                Id = activatedPhoneNumber.Id,
                PhoneNumber = activatedPhoneNumber.PhoneNumberValue,
                CustomerId = activatedPhoneNumber.CustomerId,
                Active = activatedPhoneNumber.Active
            };
        }

        return null;
    }
}
