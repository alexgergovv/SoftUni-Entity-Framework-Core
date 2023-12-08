using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.DataProcessor.ExportDto
{
    public class ExportClientDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;
        [JsonProperty("Trucks")]
        public ExportTruckDto[] Trucks { get; set; } = null!;
    }
}
