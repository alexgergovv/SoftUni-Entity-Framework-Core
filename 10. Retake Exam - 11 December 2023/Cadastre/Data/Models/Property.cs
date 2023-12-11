using Cadastre.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Data.Models
{
    public class Property
    {
        public Property()
        {
            this.PropertiesCitizens = new HashSet<PropertyCitizen>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(ValidationConstants.PropertyIdentifierMaxLength)]
        public string PropertyIdentifier { get; set; } = null!;
        [Required]
        public int Area { get; set; }
        [MaxLength(ValidationConstants.PropertyDetailsMaxLength)]
        public string? Details { get; set; }
        [Required]
        [MaxLength(ValidationConstants.PropertyAddressMaxLength)]
        public string Address { get; set; } = null!;
        [Required]
        public DateTime DateOfAcquisition { get; set; }
        [Required]
        [ForeignKey(nameof(District))]
        public int DistrictId { get; set; }
        public District District { get; set; } = null!;
        public ICollection<PropertyCitizen> PropertiesCitizens { get; set; } = null!;

    }
}
