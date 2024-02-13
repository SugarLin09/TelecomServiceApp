using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelecomService.Core.Entities
{
    public class PhoneNumber
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("PhoneNumber")]
        [JsonProperty("phoneNumberValue")]
        public string PhoneNumberValue { get; set; } = string.Empty;

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("customerId")]
        public string CustomerId { get; set; } = string.Empty;
    }
}
