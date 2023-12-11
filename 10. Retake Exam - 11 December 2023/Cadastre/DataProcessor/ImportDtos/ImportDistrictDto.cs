using Cadastre.Common;
using Cadastre.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("District")]
    public class ImportDistrictDto
    {
        [Required]
        [MinLength(ValidationConstants.DisctrictNameMinLength)]
        [MaxLength(ValidationConstants.DisctrictNameMaxLength)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;
        [Required]
        [RegularExpression(ValidationConstants.DistrictPostalCodeRegex)]
        [XmlElement("PostalCode")]
        public string PostalCode { get; set; } = null!;
        [Required]
        [XmlAttribute("Region")]
        public string Region { get; set; } = null!;
        [XmlArray("Properties")]
        public ImportPropertyDto[] Properties { get; set; } = null!;
    }
}
