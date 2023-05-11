using Microsoft.EntityFrameworkCore;
using TestDataGenerator.Models;

namespace TestDataGenerator
{
    public class IsDbContext : DbContext
    {
        // Tables
        public DbSet<Person> Persons { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<MarkReport> MarkReport { get; set; }
        public DbSet<Mark> Mark { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseNpgsql("Server=localhost;Port=8888;Database=is_data;Username=postgres;Password=123456");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mark>()
                .HasKey(e => new { e.StudentId, e.MarkReportId });

            modelBuilder.Entity<Person>()
                .Property(e => e.Birthday)
                .HasColumnType("date");

            modelBuilder.Entity<Student>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Student>()
                .Property(e => e.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Student>()
                .Property(e => e.Begin)
                .HasColumnType("date");

            modelBuilder.Entity<Student>()
                .Property(e => e.End)
                .HasColumnType("date");

            modelBuilder.Entity<MarkReport>()
                .Property(e => e.Date)
                .HasColumnType("date");

            base.OnModelCreating(modelBuilder);
        }
    }
}
