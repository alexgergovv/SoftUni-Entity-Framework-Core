﻿using P01_StudentSystem.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Homework
    {
        [Key]
        public int HomeworkId { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR")]
        public string Content { get; set; } = null!;
        public ContentType ContentType { get; set; }
        public DateTime SubmissionTime { get; set; }
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        [Required]
        public Student Student { get; set; } = null!;
        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        [Required]
        public Course Course { get; set; } = null!;
    }
}