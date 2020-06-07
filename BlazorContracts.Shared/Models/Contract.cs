using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlazorContracts.Shared.Models
{
    public class Contract
    {
        [Key]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [Required, JsonPropertyName("name")]
        public string Name { get; set; }

        [Required, Display(Name = "Phone Number"), JsonPropertyName("phonenumber")]
        public string PhoneNumber { get; set; }
    }
}