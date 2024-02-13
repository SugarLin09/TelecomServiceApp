using Microsoft.AspNetCore.Mvc;
using TelecomService.Application.Interfaces;
using TelecomService.Application.Models;

namespace TelecomService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhoneNumberController : ControllerBase
{
    private readonly IPhoneNumberService _phoneNumberService;
    private readonly ILogger<PhoneNumberController> _logger;

    public PhoneNumberController(ILogger<PhoneNumberController> logger, IPhoneNumberService phoneNumberService)
    {
        _phoneNumberService = phoneNumberService ?? throw new ArgumentNullException(nameof(phoneNumberService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet(Name = "GetAllPhoneNumbers")]
    public async Task<ActionResult<IEnumerable<PhoneNumberDto>>> GetAllPhoneNumbers()
    {
        _logger.LogInformation("[Controller {Controller}, Method {Method}]: Get all phone numbers",
            nameof(PhoneNumberController),
            nameof(GetAllPhoneNumbers));

        var phoneNumbers = await _phoneNumberService.GetAllPhoneNumbersAsync();
        return Ok(phoneNumbers);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<PhoneNumberDto>>> GetPhoneNumbersByCustomerId(string customerId)
    {
        _logger.LogInformation("[Controller {Controller}, Method {Method}]: Get all phone numbersby customer id: {customerId}",
            nameof(PhoneNumberController),
            nameof(GetPhoneNumbersByCustomerId),
            customerId);

        var phoneNumbers = await _phoneNumberService.GetPhoneNumbersByCustomerIdAsync(customerId);
        return Ok(phoneNumbers);
    }

    [HttpPost]
    public async Task<ActionResult<PhoneNumberDto>> AddPhoneNumber(PhoneNumberDto phoneNumberDto)
    {
        _logger.LogInformation("[Controller {Controller}, Method {Method}]: Add new phone number",
            nameof(PhoneNumberController),
            nameof(AddPhoneNumber));

        PhoneNumberDto phoneNumber = await _phoneNumberService.AddPhoneNumberAsync(phoneNumberDto);
        return Ok(phoneNumber);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PhoneNumberDto>> ActivatePhoneNumber(string id)
    {
        _logger.LogInformation("[Controller {Controller}, Method {Method}]: Activate phone number: {id}",
            nameof(PhoneNumberController),
            nameof(ActivatePhoneNumber), 
            id);

        PhoneNumberDto? existingPhoneNumber = await _phoneNumberService.GetPhoneNumberByIdAsync(id);
        if (existingPhoneNumber == null) return NotFound();

        PhoneNumberDto? updatedPhoneNumber = await _phoneNumberService.ActivatePhoneNumberAsync(id);
        return Ok(updatedPhoneNumber);
    }
}
