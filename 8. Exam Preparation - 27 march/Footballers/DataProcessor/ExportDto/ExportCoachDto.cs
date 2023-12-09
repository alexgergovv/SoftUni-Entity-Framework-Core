using Footballers.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class ExportCoachDto
    {
        [XmlElement("CoachName")]
        public string Name { get; set; } = null!;
        [XmlAttribute("FootballersCount")]
        public int FootballersCount { get; set; }
        [XmlArray("Footballers")]
        public ExportFootballerForCoachDto[] Footballers { get; set; }
    }
}
