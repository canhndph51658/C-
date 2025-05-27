using System;
using System.Collections.Generic;
using System.IO;

namespace CourseManagementSystem
{
     class Program
     {
          private static List<Student> students = new List<Student>(); private static List<Course> courses = new List<Course>();

          static void Main(string[] args)
          {
               InitializeCourses();
               while (true)
               {
                    DisplayMenu();
                    string choice = Console.ReadLine();
                    try
                    {
                         switch (choice)
                         {
                              case "1":
                                   AddStudent();
                                   break;
                              case "2":
                                   RegisterCourse();
                                   break;
                              case "3":
                                   InputScore();
                                   break;
                              case "4":
                                   DisplayList();
                                   break;
                              case "5":
                                   SaveToFile();
                                   break;
                              case "6":
                                   Console.WriteLine("Thoát chương trình. Tạm biệt!");
                                   return;
                              default:
                                   Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                                   break;
                         }
                    }
                    catch (Exception ex)
                    {
                         Console.WriteLine($"Lỗi: {ex.Message}");
                    }
               }
          }

          static void InitializeCourses()
          {
               courses.Add(new Course("C101", "Lập trình C# Cơ bản", CourseLevel.Beginner));
               courses.Add(new Course("C102", "Lập trình C# Nâng cao", CourseLevel.Intermediate));
               courses.Add(new Course("C103", "Lập trình C# Chuyên sâu", CourseLevel.Advanced));
          }

          static void DisplayMenu()
          {
               Console.WriteLine("\n=== HỆ THỐNG QUẢN LÝ HỌC VIÊN ===");
               Console.WriteLine("1. Thêm học viên");
               Console.WriteLine("2. Đăng ký khóa học");
               Console.WriteLine("3. Nhập điểm");
               Console.WriteLine("4. Hiển thị danh sách");
               Console.WriteLine("5. Ghi dữ liệu ra file");
               Console.WriteLine("6. Thoát");
               Console.Write("Nhập lựa chọn của bạn: ");
          }

          static void AddStudent()
          {
               Console.Write("Nhập họ tên: ");
               string fullName = Console.ReadLine();
               Console.Write("Nhập email: ");
               string email = Console.ReadLine();
               Console.Write("Nhập tên đăng nhập: ");
               string username = Console.ReadLine();
               Console.Write("Nhập mật khẩu: ");
               string password = Console.ReadLine();

               Student student = new Student(fullName, email, username, password);
               students.Add(student);
               Console.WriteLine("Thêm học viên thành công!");
          }

          static void RegisterCourse()
          {
               Console.Write("Nhập email học viên: ");
               string email = Console.ReadLine();
               Student student = students.Find(s => s.Email == email);
               if (student == null)
                    throw new Exception("Không tìm thấy học viên!");

               Console.WriteLine("Danh sách khóa học:");
               foreach (var Course in courses)
               {
                    Console.WriteLine($"{Course.CourseId}: {Course.CourseName} ({Course.Level})");
               }
               Console.Write("Nhập mã khóa học: ");
               string courseId = Console.ReadLine();
               Course course = courses.Find(c => c.CourseId == courseId);
               if (course == null)
                    throw new Exception("Không tìm thấy khóa học!");

               student.RegisterCourse(course);
               Console.WriteLine("Đăng ký khóa học thành công!");
          }

          static void InputScore()
          {
               Console.Write("Nhập email học viên: ");
               string email = Console.ReadLine();
               Student student = students.Find(s => s.Email == email);
               if (student == null)
                    throw new Exception("Không tìm thấy học viên!");

               Console.Write("Nhập mã khóa học: ");
               string courseId = Console.ReadLine();
               Console.Write("Nhập điểm (0-100): ");
               if (!double.TryParse(Console.ReadLine(), out double score))
                    throw new Exception("Điểm không hợp lệ!");

               student.TakeExam(courseId, score);
               Console.WriteLine("Nhập điểm thành công!");
          }

          static void DisplayList()
          {
               foreach (var student in students)
               {
                    student.DisplayInfo();
               }
          }

          static void SaveToFile()
          {
               try
               {
                    using (StreamWriter writer = new StreamWriter("students.csv"))
                    {
                         writer.WriteLine("Email,FullName,CourseId,Score,Level");
                         foreach (var student in students)
                         {
                              foreach (var enrollment in student.GetEnrollments())
                              {
                                   writer.WriteLine($"{student.Email},{student.FullName},{enrollment.Course.CourseId},{enrollment.Score},{enrollment.Level}");
                              }
                         }
                    }
                    Console.WriteLine("Ghi dữ liệu ra file thành công!");
               }
               catch (IOException ex)
               {
                    throw new Exception("Lỗi khi ghi file: " + ex.Message);
               }
          }
     }
}