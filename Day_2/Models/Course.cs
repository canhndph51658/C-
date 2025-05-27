namespace CourseManagementSystem
{
     public class Course
     {
          private string courseId; private string courseName; private CourseLevel level;

          public Course(string courseId, string courseName, CourseLevel level)
          {
               this.courseId = courseId;
               this.courseName = courseName;
               this.level = level;
          }

          public string CourseId
          {
               get => courseId;
               set => courseId = value;
          }

          public string CourseName
          {
               get => courseName;
               set => courseName = value;
          }

          public CourseLevel Level
          {
               get => level;
               set => level = value;
          }
     }
}