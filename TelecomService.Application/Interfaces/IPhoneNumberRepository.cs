using TelecomService.Core.Entities;

namespace TelecomService.Application.Interfaces
{
    public interface IPhoneNumberRepository
    {
        Task<IEnumerable<PhoneNumber>> GetAllPhoneNumbersAsync();
        Task<PhoneNumber?> GetPhoneNumberByIdAsync(string phoneNumberId);
        Task<IEnumerable<PhoneNumber>> GetPhoneNumbersByCustomerIdAsync(string customerId);
        Task<PhoneNumber> AddPhoneNumberAsync(PhoneNumber newPhoneNumber);
        Task<PhoneNumber> ActivatePhoneNumberAsync(PhoneNumber activatedPhoneNumber);
    }
}
