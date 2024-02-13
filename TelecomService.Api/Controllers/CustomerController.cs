using Microsoft.AspNetCore.Mvc;
using TelecomService.Application.Interfaces;
using TelecomService.Core.Entities;

namespace TelecomService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
    {
        _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet(Name = "GetAllCustomers")]
    public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
    {
        _logger.LogInformation("[Controller {Controller}, Method {Method}]: Get all customers",
            nameof(CustomerController),
            nameof(GetAllCustomers));

        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }
}

