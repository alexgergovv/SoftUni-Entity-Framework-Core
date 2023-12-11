using Cadastre.Common;
using Cadastre.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Data.Models
{
    public class District
    {
        public District()
        {
            this.Properties = new HashSet<Property>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(ValidationConstants.DisctrictNameMaxLength)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(ValidationConstants.DistrictPostalCodeMaxLength)]
        public string PostalCode { get; set; } = null!;
        [Required]
        public Region Region { get; set; }
        public ICollection<Property> Properties { get; set; } = null!;
    }
}
