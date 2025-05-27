using Microsoft.EntityFrameworkCore;
using TrainingCenterManagement.Data;
using TrainingCenterManagement.Models;

namespace TrainingCenterManagement
{
     class Program
     {
          static async Task Main(string[] args)
          {
               // Cấu hình DbContext với SQLite
               var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
               optionsBuilder.UseSqlite("Data Source=training_center.db");

               using var context = new AppDbContext(optionsBuilder.Options);

               // Đảm bảo database được tạo
               await context.Database.EnsureCreatedAsync();

               // Thêm dữ liệu mẫu
               await AddSampleData(context);

               // Thực hiện các thao tác CRUD và truy vấn
               await PerformCrudAndQueries(context);
          }

          static async Task AddSampleData(AppDbContext context)
          {
               // Thêm học viên
               var student1 = new Student { FullName = "Nguyễn Văn A", Email = "a@gmail.com", BirthDate = new DateTime(2000, 1, 1) };
               var student2 = new Student { FullName = "Trần Thị B", Email = "b@gmail.com", BirthDate = new DateTime(1999, 5, 10) };
               context.Students.AddRange(student1, student2);

               // Thêm khóa học
               var course1 = new Course { Title = "Lập trình C#", Level = "Cơ bản", Duration = 60 };
               var course2 = new Course { Title = "Lập trình Python", Level = "Nâng cao", Duration = 80 };
               context.Courses.AddRange(course1, course2);

               // Thêm đăng ký khóa học
               var enrollment1 = new Enrollment { Student = student1, Course = course1, EnrollDate = DateTime.UtcNow };
               var enrollment2 = new Enrollment { Student = student2, Course = course1, EnrollDate = DateTime.UtcNow };
               context.Enrollments.AddRange(enrollment1, enrollment2);

               await context.SaveChangesAsync();
          }

          static async Task PerformCrudAndQueries(AppDbContext context)
          {
               // 1. Lấy danh sách học viên của khóa học "Lập trình C#"
               Console.WriteLine("\nHọc viên trong khóa 'Lập trình C#':");
               var studentsInCourse = await context.Students
                   .Include(s => s.Enrollments)
                   .ThenInclude(e => e.Course)
                   .Where(s => s.Enrollments.Any(e => e.Course.Title == "Lập trình C#"))
                   .AsNoTracking()
                   .ToListAsync();

               foreach (var student in studentsInCourse)
               {
                    Console.WriteLine($"Học viên: {student.FullName}, Email: {student.Email}");
               }

               // 2. Lấy danh sách khóa học có nhiều hơn 1 học viên
               Console.WriteLine("\nKhóa học có nhiều hơn 1 học viên:");
               var popularCourses = await context.Courses
                   .Include(c => c.Enrollments)
                   .Where(c => c.Enrollments.Count > 1)
                   .Select(c => new { c.Title, EnrollmentCount = c.Enrollments.Count })
                   .AsNoTracking()
                   .ToListAsync();

               foreach (var course in popularCourses)
               {
                    Console.WriteLine($"Khóa học: {course.Title}, Số học viên: {course.EnrollmentCount}");
               }

               // 3. Lọc học viên theo ngày sinh (sau năm 1999)
               Console.WriteLine("\nHọc viên sinh sau 1999:");
               var youngStudents = await context.Students
                   .Where(s => s.BirthDate.Year > 1999)
                   .OrderBy(s => s.FullName)
                   .AsNoTracking()
                   .ToListAsync();

               foreach (var student in youngStudents)
               {
                    Console.WriteLine($"Học viên: {student.FullName}, Ngày sinh: {student.BirthDate.ToShortDateString()}");
               }

               // 4. Sửa thông tin học viên
               var studentToUpdate = await context.Students.FirstOrDefaultAsync(s => s.Email == "a@gmail.com");
               if (studentToUpdate != null)
               {
                    studentToUpdate.FullName = "Nguyễn Văn A Cập Nhật";
                    await context.SaveChangesAsync();
                    Console.WriteLine("\nĐã cập nhật thông tin học viên.");
               }

               // 5. Xóa một học viên
               var studentToDelete = await context.Students.FirstOrDefaultAsync(s => s.Email == "b@gmail.com");
               if (studentToDelete != null)
               {
                    context.Students.Remove(studentToDelete);
                    await context.SaveChangesAsync();
                    Console.WriteLine("\nĐã xóa học viên.");
               }
          }
     }
}