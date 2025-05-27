using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TrainingCenterManagement.Data;

namespace TrainingCenterManagement.Data
{
     public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
     {
          public AppDbContext CreateDbContext(string[] args)
          {
               var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
               optionsBuilder.UseSqlite("Data Source=training_center.db");

               return new AppDbContext(optionsBuilder.Options);
          }
     }
}