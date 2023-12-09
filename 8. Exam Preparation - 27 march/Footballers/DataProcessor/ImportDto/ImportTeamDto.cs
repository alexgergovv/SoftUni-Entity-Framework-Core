using Footballers.Common;
using Footballers.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamDto
    {
        [Required]
        [MinLength(ValidationConstants.TeamNameMinLength)]
        [MaxLength(ValidationConstants.TeamNameMaxLength)]
        [RegularExpression(@"^[a-zA-Z0-9\s\.\-]+$")]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;
        [Required]
        [MinLength(ValidationConstants.TeamNationalityMinLength)]
        [MaxLength(ValidationConstants.TeamNationalityMaxLength)]
        [JsonProperty("Nationality")]
        public string Nationality { get; set; } = null!;
        [Required]
        [JsonProperty("Trophies")]
        public int Trophies { get; set; }
        [JsonProperty("Footballers")]
        public int[] FootballerIds { get; set; }
    }
}
