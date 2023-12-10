﻿using Boardgames.Common;
using Boardgames.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class ExportCreatorDto
    {
        [XmlElement("CreatorName")]
        public string Name { get; set; } = null!;
        [XmlAttribute("BoardgamesCount")]
        public int BoardgamesCount { get; set; }
        [XmlArray("Boardgames")]
        public ExportBoardgameForCreatorDto[] Boardgames { get; set; } = null!;
    }
}
