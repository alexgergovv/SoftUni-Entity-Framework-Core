﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType("Truck")]
    public class ExportTrucksForDespatcherDto
    {
        [XmlElement("RegistrationNumber")]
        public string? RegistrationNumber { get; set; }
        [XmlElement("Make")]
        public string MakeType { get; set; }
    }
}
