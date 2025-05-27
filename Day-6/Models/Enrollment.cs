using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingCenterManagement.Models
{
     public class Enrollment
     {
          [Key]
          public int Id { get; set; }

          [Required]
          public int StudentId { get; set; }

          [Required]
          public int CourseId { get; set; }

          [Required]
          [DataType(DataType.Date)]
          public DateTime EnrollDate { get; set; }

          public DateTime CreatedAt { get; set; }
          public DateTime UpdatedAt { get; set; }

          [ForeignKey("StudentId")]
          public Student Student { get; set; }

          [ForeignKey("CourseId")]
          public Course Course { get; set; }
     }
}