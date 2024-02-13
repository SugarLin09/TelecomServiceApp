using TelecomService.Application.Models;
using TelecomService.Core.Entities;

namespace TelecomService.Application.Interfaces
{
    public interface IPhoneNumberService
    {
        Task<IEnumerable<PhoneNumberDto>> GetAllPhoneNumbersAsync();
        Task<PhoneNumberDto?> GetPhoneNumberByIdAsync(string phoneNumberId);
        Task<IEnumerable<PhoneNumberDto>> GetPhoneNumbersByCustomerIdAsync(string customerId);
        Task<PhoneNumberDto> AddPhoneNumberAsync(PhoneNumberDto newPhoneNumberDto);
        Task<PhoneNumberDto?> ActivatePhoneNumberAsync(string phoneNumberId);
    }
}
