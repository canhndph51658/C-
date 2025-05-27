namespace CourseManagementSystem
{
     public abstract class Person
     {
          private string fullName; private string email;

          public Person(string fullName, string email)
          {
               this.fullName = fullName;
               this.email = email;
          }

          public string FullName
          {
               get => fullName;
               set => fullName = value;
          }

          public string Email
          {
               get => email;
               set => email = value;
          }

          public virtual void DisplayInfo()
          {
               Console.WriteLine($"Họ tên: {FullName}, Email: {Email}");
          }
     }
}