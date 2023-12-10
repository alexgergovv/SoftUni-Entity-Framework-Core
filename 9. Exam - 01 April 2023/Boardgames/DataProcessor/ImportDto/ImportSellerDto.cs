using Boardgames.Common;
using Boardgames.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellerDto
    {
        [Required]
        [MinLength(ValidationConstants.SellerNameMinLength)]
        [MaxLength(ValidationConstants.SellerNameMaxLength)]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(ValidationConstants.SellerAddressMinLength)]
        [MaxLength(ValidationConstants.SellerAddressMaxLength)]
        [JsonProperty("Address")]
        public string Address { get; set; } = null!;
        [Required]
        [JsonProperty("Country")]
        public string Country { get; set; } = null!;
        [Required]
        [RegularExpression(ValidationConstants.SellerWebsiteRegex)]
        [JsonProperty("Website")]
        public string Website { get; set; } = null!;
        [JsonProperty("Boardgames")]
        public int[] BoardgameIds { get; set; } = null!;
    }
}
