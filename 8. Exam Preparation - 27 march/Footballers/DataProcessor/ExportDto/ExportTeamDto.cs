using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Footballers.DataProcessor.ExportDto
{
    public class ExportTeamDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;
        [JsonProperty("Footballers")]
        public ExportFootballerDto[] Footballers { get; set; }
    }
}
