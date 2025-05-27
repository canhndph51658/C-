using Microsoft.EntityFrameworkCore;
using TrainingCenterManagement.Models;

namespace TrainingCenterManagement.Data
{
     public class AppDbContext : DbContext
     {
          public DbSet<Student> Students { get; set; }
          public DbSet<Course> Courses { get; set; }
          public DbSet<Enrollment> Enrollments { get; set; }

          public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
          {
          }

          protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
               modelBuilder.Entity<Enrollment>()
                   .HasOne(e => e.Student)
                   .WithMany(s => s.Enrollments)
                   .HasForeignKey(e => e.StudentId);

               modelBuilder.Entity<Enrollment>()
                   .HasOne(e => e.Course)
                   .WithMany(c => c.Enrollments)
                   .HasForeignKey(e => e.CourseId);

               modelBuilder.Entity<Enrollment>()
                   .HasKey(e => e.Id);
          }

          public override int SaveChanges()
          {
               UpdateTimestamps();
               return base.SaveChanges();
          }

          public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
          {
               UpdateTimestamps();
               return base.SaveChangesAsync(cancellationToken);
          }

          private void UpdateTimestamps()
          {
               var entries = ChangeTracker.Entries()
                   .Where(e => e.Entity is Student || e.Entity is Course || e.Entity is Enrollment)
                   .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

               foreach (var entry in entries)
               {
                    var now = DateTime.UtcNow;

                    if (entry.State == EntityState.Added)
                    {
                         entry.Property("CreatedAt").CurrentValue = now;
                    }
                    entry.Property("UpdatedAt").CurrentValue = now;
               }
          }
     }
}