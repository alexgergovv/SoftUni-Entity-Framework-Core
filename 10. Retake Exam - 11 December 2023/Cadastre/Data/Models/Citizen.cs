﻿using Cadastre.Common;
using Cadastre.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Data.Models
{
    public class Citizen
    {
        public Citizen()
        {
            this.PropertiesCitizens = new HashSet<PropertyCitizen>();
        }
        [Key] 
        public int Id { get; set; }
        [Required]
        [MaxLength(ValidationConstants.CitizenFirstNameMaxLength)]
        public string FirstName { get; set; } = null!;
        [Required]
        [MaxLength(ValidationConstants.CitizenLastNameMaxLength)]
        public string LastName { get; set; } = null!;
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public MaritalStatus MaritalStatus { get; set; }
        public ICollection<PropertyCitizen> PropertiesCitizens { get; set; } = null!;
    }
}
