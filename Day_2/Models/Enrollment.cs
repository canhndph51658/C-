namespace CourseManagementSystem
{
     public class Enrollment
     {
          private Course course; private double score; private string level;


          public Enrollment(Course course)
          {
               this.course = course;
               this.score = 0;
               CalculateLevel();
          }

          public Course Course
          {
               get => course;
          }

          public double Score
          {
               get => score;
               set
               {
                    score = value;
                    CalculateLevel();
               }
          }

          public string Level
          {
               get => level;
          }

          private void CalculateLevel()
          {
               if (score >= 80)
                    level = "Xuất sắc";
               else if (score >= 60)
                    level = "Khá";
               else if (score >= 40)
                    level = "Trung bình";
               else
                    level = "Yếu";
          }
     }
}