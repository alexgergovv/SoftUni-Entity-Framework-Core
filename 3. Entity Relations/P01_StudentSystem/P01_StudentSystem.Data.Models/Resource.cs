using P01_StudentSystem.Data.Common;
using P01_StudentSystem.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }
        [Required]
        [MaxLength(ValidationConstants.ResourceNameMaxLength)]
        [Column(TypeName = "NVARCHAR")]
        public string Name { get; set; } = null!;
        [Required]
        [Column(TypeName = "VARCHAR")]
        public string Url { get; set; } = null!;
        public ResourceType ResourceType { get; set; }
        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        [Required]
        public virtual Course Course { get; set; } = null!;
    }
}