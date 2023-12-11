using Cadastre.Data.Enumerations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataProcessor.ExportDtos
{
    public class ExportCitizenDto
    {
        [JsonProperty("LastName")]
        public string LastName { get; set; } = null!;
        [JsonProperty("MaritalStatus")]
        public string MaritalStatus { get; set; } = null!;
    }
}
