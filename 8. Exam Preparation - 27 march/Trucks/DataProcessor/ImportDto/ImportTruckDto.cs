using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trucks.Common;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class ImportTruckDto
    {
        [MinLength(ValidationConstants.TruckRegistrationNumberMaxLength)]
        [MaxLength(ValidationConstants.TruckRegistrationNumberMaxLength)]
        [RegularExpression(ValidationConstants.TruckRegistrationNumberRegex)]
        [XmlElement("RegistrationNumber")]
        public string? RegistrationNumber { get; set; }
        [Required]
        [MinLength(ValidationConstants.TruckVinNumberMaxLength)]
        [MaxLength(ValidationConstants.TruckVinNumberMaxLength)]
        [XmlElement("VinNumber")]
        public string VinNumber { get; set; } = null!;
        [XmlElement("TankCapacity")]
        [Range(ValidationConstants.TruckTankCapacityMinValue, ValidationConstants.TruckTankCapacityMaxValue)]
        public int TankCapacity { get; set; }
        [XmlElement("CargoCapacity")]
        [Range(ValidationConstants.TruckCargoCapacityMinValue, ValidationConstants.TruckCargoCapacityMaxValue)]
        public int CargoCapacity { get; set; }
        [Required]
        [XmlElement("CategoryType")]
        [Range(ValidationConstants.TruckCategoryTypeMinValue, ValidationConstants.TruckCategoryTypeMaxValue)]
        public int CategoryType { get; set; }
        [Required]
        [XmlElement("MakeType")]
        [Range(ValidationConstants.TruckMakeTypeMinValue, ValidationConstants.TruckMakeTypeMaxValue)]
        public int MakeType { get; set; }
    }
}
