using Footballers.Common;
using Footballers.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class ImportFootballerDto
    {
        [Required]
        [MinLength(ValidationConstants.FootballerNameMinLength)]
        [MaxLength(ValidationConstants.FootballerNameMaxLength)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;
        [Required]
        [XmlElement("ContractStartDate")]
        public string ContractStartDate { get; set; }
        [Required]
        [XmlElement("ContractEndDate")]
        public string ContractEndDate { get; set; }
        [Required]
        [Range(ValidationConstants.FootballerPositionMinValue, ValidationConstants.FootballerPositionMaxValue)]
        [XmlElement("PositionType")]
        public int PositionType { get; set; }
        [Required]
        [Range(ValidationConstants.FootballerBestSkillMinValue, ValidationConstants.FootballerBestSkillMaxValue)]
        [XmlElement("BestSkillType")]
        public int BestSkillType { get; set; }
    }
}
