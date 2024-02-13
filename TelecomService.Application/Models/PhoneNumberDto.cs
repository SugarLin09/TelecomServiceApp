namespace TelecomService.Application.Models
{
    public class PhoneNumberDto
    {
        public string? Id { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public bool Active { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public string? CustomerName { get; set; }
    }
}
