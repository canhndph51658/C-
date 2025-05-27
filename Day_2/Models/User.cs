namespace CourseManagementSystem
{
     public abstract class User
     {
          protected string Username { get; set; }
          protected string Password { get; set; }

          public User(string username, string password)
          {
               Username = username;
               Password = password;
          }

          public abstract void Login();
          public abstract void Logout();
     }
}