using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainingCenterManagement.Models
{
     public class Course
     {
          [Key]
          public int Id { get; set; }

          [Required(ErrorMessage = "Tên khóa học không được để trống")]
          [MaxLength(200, ErrorMessage = "Tên khóa học không được dài quá 200 ký tự")]
          public string Title { get; set; }

          [Required(ErrorMessage = "Cấp độ không được để trống")]
          [MaxLength(50, ErrorMessage = "Cấp độ không được dài quá 50 ký tự")]
          public string Level { get; set; }

          [Range(1, 1000, ErrorMessage = "Thời lượng phải từ 1 đến 1000 giờ")]
          public int Duration { get; set; }

          public DateTime CreatedAt { get; set; }
          public DateTime UpdatedAt { get; set; }

          public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
     }
}