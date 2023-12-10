using Boardgames.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Boardgames.DataProcessor.ExportDto
{
    public class ExportSellerDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;
        [JsonProperty("Website")]
        public string Website { get; set; } = null!;
        [JsonProperty("Boardgames")]
        public ExportBoardgameDto[] BoardgamesSellers { get; set; } = null!;
    }
}
