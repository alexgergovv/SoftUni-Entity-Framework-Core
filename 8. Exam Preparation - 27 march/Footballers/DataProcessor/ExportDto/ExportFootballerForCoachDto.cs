using Footballers.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Footballer")]
    public class ExportFootballerForCoachDto
    {
        [XmlElement("Name")]
        public string Name { get; set; } = null!;
        [XmlElement("Position")]
        public string Position { get; set; }
    }
}
