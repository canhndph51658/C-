namespace Day_7.Models
{
     public class TodoItem
     {
          public int Id { get; set; }
          public string Title { get; set; } = string.Empty;
          public DateTime Deadline { get; set; }
          public bool IsCompleted { get; set; }
     }
}
