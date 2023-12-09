using Footballers.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    public class ExportFootballerDto
    {
        [JsonProperty("FootballerName")]
        public string Name { get; set; } = null!;
        [JsonProperty("ContractStartDate")]
        public string ContractStartDate { get; set; }
        [JsonProperty("ContractEndDate")]
        public string ContractEndDate { get; set; }
        [JsonProperty("BestSkillType")]
        public string BestSkillType { get; set; }
        [JsonProperty("PositionType")]
        public string PositionType { get; set; }
    }
}
