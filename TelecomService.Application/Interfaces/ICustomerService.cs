using TelecomService.Core.Entities;

namespace TelecomService.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
    }
}
