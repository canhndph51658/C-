using System;
using System.Collections.Generic;

namespace CourseManagementSystem
{
     public class Student : Person, ICanLearn
     {
          private List<Enrollment> enrollments; public string Username { get; set; }
          public string Password { get; set; }

          public Student(string fullName, string email, string username, string password)
    : base(fullName, email)
          {
               enrollments = new List<Enrollment>();
               Username = username;
               Password = password;
          }

          public override void DisplayInfo()
          {
               Console.WriteLine("\n=== Thông tin học viên ===");
               base.DisplayInfo();
               Console.WriteLine("Các khóa học đã đăng ký:");
               foreach (var enrollment in enrollments)
               {
                    Console.WriteLine($" - {enrollment.Course.CourseName} ({enrollment.Course.Level}): Điểm {enrollment.Score}, Xếp loại: {enrollment.Level}");
               }
          }

          public void RegisterCourse(Course course)
          {
               if (enrollments.Exists(e => e.Course.CourseId == course.CourseId))
                    throw new Exception("Học viên đã đăng ký khóa học này!");
               enrollments.Add(new Enrollment(course));
          }

          public void TakeExam(string courseId, double score)
          {
               if (score < 0 || score > 100)
                    throw new Exception("Điểm phải từ 0 đến 100!");
               var enrollment = enrollments.Find(e => e.Course.CourseId == courseId);
               if (enrollment == null)
                    throw new Exception("Học viên chưa đăng ký khóa học này!");
               enrollment.Score = score;
          }

          public void Login()
          {
               Console.WriteLine($"Học viên {FullName} đã đăng nhập.");
          }

          public void Logout()
          {
               Console.WriteLine($"Học viên {FullName} đã đăng xuất.");
          }

          public List<Enrollment> GetEnrollments()
          {
               return enrollments;
          }
     }
}