using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TelecomService.Application.Interfaces;
using TelecomService.Core.Entities;
using TelecomService.Infrastructure.Data;

namespace TelecomService.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<CustomerRepository> _logger;

    public CustomerRepository(ILogger<CustomerRepository> logger, AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        _logger.LogInformation("[Repository {Repository}, Method {Method}]: Get all customers",
            nameof(CustomerRepository),
            nameof(GetAllCustomersAsync));

        return await _context.Customers.ToListAsync();

    }

    public async Task<Customer?> GetCustomerByIdAsync(string customerId)
    {
        _logger.LogInformation("[Repository {Repository}, Method {Method}]: Get customer by id: {customerId}",
            nameof(CustomerRepository),
            nameof(GetAllCustomersAsync),
            customerId);

        return await _context.Customers.FindAsync(customerId);

    }
}
