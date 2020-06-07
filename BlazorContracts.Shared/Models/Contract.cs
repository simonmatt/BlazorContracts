using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlazorContracts.Shared.Models
{
    public class Contract
    {
        [Key]
        //[JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        //[Required, JsonPropertyName("name")]
        public string Name { get; set; }

        [Required]
        //[Required, Display(Name = "Phone Number"), JsonPropertyName("phonenumber")]
        public string PhoneNumber { get; set; }
    }
}