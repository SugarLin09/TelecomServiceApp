using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TelecomService.Application.Interfaces;
using TelecomService.Core.Entities;
using TelecomService.Infrastructure.Data;

namespace TelecomService.Infrastructure.Repositories;

public class PhoneNumberRepository: IPhoneNumberRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<PhoneNumberRepository> _logger;

    public PhoneNumberRepository(ILogger<PhoneNumberRepository> logger, AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<PhoneNumber>> GetAllPhoneNumbersAsync()
    {
        _logger.LogInformation("[Repository {Repository}, Method {Method}]: Get all phone numbers",
            nameof(PhoneNumberRepository),
            nameof(GetAllPhoneNumbersAsync));

        var result = await _context.PhoneNumbers.ToListAsync();

        return result;
    }

    public async Task<PhoneNumber?> GetPhoneNumberByIdAsync(string phoneNumberId)
    {
        _logger.LogInformation("[Repository {Repository}, Method {Method}]: Get phone number by id: {phoneNumberId}",
            nameof(PhoneNumberRepository),
            nameof(GetPhoneNumberByIdAsync),
            phoneNumberId);

        return await _context.PhoneNumbers.FindAsync(phoneNumberId);
    }

    public async Task<IEnumerable<PhoneNumber>> GetPhoneNumbersByCustomerIdAsync(string customerId)
    {
        _logger.LogInformation("[Repository {Repository}, Method {Method}]: Get phone numbers by customer id: {customerId}",
            nameof(PhoneNumberRepository),
            nameof(GetPhoneNumbersByCustomerIdAsync),
            customerId);

        IQueryable<PhoneNumber> query = _context.PhoneNumbers.Where(phoneNumber => phoneNumber.CustomerId == customerId);

        return await query.ToListAsync();
    }

    public async Task<PhoneNumber> AddPhoneNumberAsync(PhoneNumber newPhoneNumber)
    {
        _logger.LogInformation("[Repository {Repository}, Method {Method}]: Add new phone number",
            nameof(PhoneNumberRepository),
            nameof(AddPhoneNumberAsync));

        await _context.PhoneNumbers.AddAsync(newPhoneNumber);
        await _context.SaveChangesAsync();
        return newPhoneNumber;
    }

    public async Task<PhoneNumber> ActivatePhoneNumberAsync(PhoneNumber activatedPhoneNumber)
    {
        _logger.LogInformation("[Repository {Repository}, Method {Method}]: Activate phone number",
            nameof(PhoneNumberRepository),
            nameof(ActivatePhoneNumberAsync));

        _context.PhoneNumbers.Update(activatedPhoneNumber);
        await _context.SaveChangesAsync();
        return activatedPhoneNumber;
    }
}
