using Cadastre.Common;
using Cadastre.Data.Enumerations;
using Cadastre.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataProcessor.ImportDtos
{
    [JsonObject("Citizen")]
    public class ImportCitizenDto
    {
        [Required]
        [MinLength(ValidationConstants.CitizenFirstNameMinLength)]
        [MaxLength(ValidationConstants.CitizenFirstNameMaxLength)]
        [JsonProperty("FirstName")]
        public string FirstName { get; set; } = null!;
        [Required]
        [MinLength(ValidationConstants.CitizenLastNameMinLength)]
        [MaxLength(ValidationConstants.CitizenLastNameMaxLength)]
        [JsonProperty("LastName")]
        public string LastName { get; set; } = null!;
        [Required]
        [JsonProperty("BirthDate")]
        public string BirthDate { get; set; } = null!;
        [Required]
        [EnumDataType(typeof(MaritalStatus))]
        [JsonProperty("MaritalStatus")]
        public string MaritalStatus { get; set; } = null!;
        [JsonProperty("Properties")]
        public int[] PropertiesIds { get; set; } = null!;
    }
}
