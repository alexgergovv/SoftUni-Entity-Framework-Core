using Boardgames.Data.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Boardgames.DataProcessor.ExportDto
{
    public class ExportBoardgameDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;
        [JsonProperty("Rating")]
        public double Rating { get; set; }
        [JsonProperty("Mechanics")]
        public string Mechanics { get; set; } = null!;
        [JsonProperty("Category")]
        public string CategoryType { get; set; } = null!;
    }
}
