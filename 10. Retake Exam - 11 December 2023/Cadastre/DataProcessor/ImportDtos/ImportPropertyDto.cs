using Cadastre.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("Property")]
    public class ImportPropertyDto
    {
        [Required]
        [MinLength(ValidationConstants.PropertyIdentifierMinLength)]
        [MaxLength(ValidationConstants.PropertyIdentifierMaxLength)]
        [XmlElement("PropertyIdentifier")]
        public string PropertyIdentifier { get; set; } = null!;
        [Required]
        [Range(ValidationConstants.PropertyAreaMinValue, int.MaxValue)]
        [XmlElement("Area")]
        public int Area { get; set; }
        [MinLength(ValidationConstants.PropertyDetailsMinLength)]
        [MaxLength(ValidationConstants.PropertyDetailsMaxLength)]
        [XmlElement("Details")]
        public string? Details { get; set; }
        [Required]
        [MinLength(ValidationConstants.PropertyAddressMinLength)]
        [MaxLength(ValidationConstants.PropertyAddressMaxLength)]
        [XmlElement("Address")]
        public string Address { get; set; } = null!;
        [Required]
        [XmlElement("DateOfAcquisition")]
        public string DateOfAcquisition { get; set; } = null!;
    }
}
