using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Common;
using Trucks.Data.Models.Enums;
using Trucks.Data.Models;
using Newtonsoft.Json;

namespace Trucks.DataProcessor.ExportDto
{
    public class ExportTruckDto
    {
        [JsonProperty("TruckRegistrationNumber")]
        public string? RegistrationNumber { get; set; }
        [JsonProperty("VinNumber")]
        public string VinNumber { get; set; } = null!;
        [JsonProperty("TankCapacity")]
        public int TankCapacity { get; set; }
        [JsonProperty("CargoCapacity")]
        public int CargoCapacity { get; set; }
        [JsonProperty("CategoryType")]
        public string CategoryType { get; set; }
        [JsonProperty("MakeType")]
        public string MakeType { get; set; }
    }
}
