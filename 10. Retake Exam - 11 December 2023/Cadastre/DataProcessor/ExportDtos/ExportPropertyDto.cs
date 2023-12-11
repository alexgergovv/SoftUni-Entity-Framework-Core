using Cadastre.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataProcessor.ExportDtos
{
    public class ExportPropertyDto
    {
        [JsonProperty("PropertyIdentifier")]
        public string PropertyIdentifier { get; set; } = null!;
        [JsonProperty("Area")]
        public int Area { get; set; }
        [JsonProperty("Address")]
        public string Address { get; set; } = null!;
        [JsonProperty("DateOfAcquisition")]
        public string DateOfAcquisition { get; set; } = null!;
        [JsonProperty("Owners")]
        public ExportCitizenDto[] Owners { get; set; } = null!;
    }
}
