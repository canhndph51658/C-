using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingCenterManagement.Models
{
     public class Student
     {
          [Key]
          public int Id { get; set; }

          [Required(ErrorMessage = "Họ tên không được để trống")]
          [MaxLength(100, ErrorMessage = "Họ tên không được dài quá 100 ký tự")]
          public string FullName { get; set; }

          [Required(ErrorMessage = "Email không được để trống")]
          [EmailAddress(ErrorMessage = "Email không hợp lệ")]
          public string Email { get; set; }

          [Required(ErrorMessage = "Ngày sinh không được để trống")]
          [DataType(DataType.Date)]
          [Range(typeof(DateTime), "1/1/1900", "12/31/2025", ErrorMessage = "Ngày sinh không hợp lệ")]
          public DateTime BirthDate { get; set; }

          public DateTime CreatedAt { get; set; }
          public DateTime UpdatedAt { get; set; }

          public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
     }
}