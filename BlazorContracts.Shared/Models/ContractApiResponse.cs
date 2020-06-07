using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BlazorContracts.Shared.Models
{
    public class ContractApiResponse
    {
        [JsonPropertyName("@odata.count")]
        public int Count { get; set; }

        [JsonPropertyName("value")]
        public List<Contract> Contracts { get; set; }
    }
}
