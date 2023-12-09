﻿using Footballers.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class ImportCoachDto
    {
        [Required]
        [XmlElement("Name")]
        [MinLength(ValidationConstants.CoachNameMinLength)]
        [MaxLength(ValidationConstants.CoachNameMaxLength)]
        public string Name { get; set; } = null!;
        [Required]
        [XmlElement("Nationality")]
        public string Nationality { get; set; } = null!;
        [XmlArray("Footballers")]
        public ImportFootballerDto[] Footballers { get; set; }
    }
}
