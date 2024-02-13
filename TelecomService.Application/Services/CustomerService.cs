using Microsoft.Extensions.Logging;
using TelecomService.Application.Interfaces;
using TelecomService.Core.Entities;

namespace TelecomService.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(ILogger<CustomerService> logger, ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        _logger.LogInformation("[Service {Service}, Method {Method}]: Get all customers",
            nameof(CustomerService),
            nameof(GetAllCustomersAsync));

        var customers = await _customerRepository.GetAllCustomersAsync();
        return customers;
    }
}
