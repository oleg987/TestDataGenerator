using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataGenerator.Models;

namespace TestDataGenerator
{
    public class IsDbContext : DbContext
    {
        // Tables
        public DbSet<Person> Persons { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<StudyFlow> StudyFlow { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Country> Countries { get; set; }       
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<EmployeeSubject> EmployeeSubjects { get; set; }
        public DbSet<MarkReport> MarkReport { get; set; }
        public DbSet<Mark> Mark { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=8888;Database=is_data;Username=postgres;Password=123456");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
