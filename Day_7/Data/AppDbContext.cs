using Microsoft.EntityFrameworkCore;
using Day_7.Models;

namespace Day_7.Data
{
     public class AppDbContext : DbContext
     {
          public DbSet<TodoItem> TodoItems { get; set; }

          protected override void OnConfiguring(DbContextOptionsBuilder options)
          {
               options.UseSqlite("Data Source=todo.db");
          }
     }
}
