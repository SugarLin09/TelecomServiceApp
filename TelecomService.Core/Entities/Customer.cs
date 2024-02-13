using Newtonsoft.Json;

namespace TelecomService.Core.Entities
{
    public class Customer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}
