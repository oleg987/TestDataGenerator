using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TestDataGenerator.Models;

namespace TestDataGenerator.Generators
{
    public class AlternativeMarkReportGenerator : GeneratorBase
    {
        public AlternativeMarkReportGenerator(string connectionString) : base(connectionString)
        {
            
        }

        public void Generate()
        {
            var sw = new Stopwatch();
            sw.Start();

            var subjects = _ctx.Subjects
                .AsNoTracking()
                .AsSplitQuery()
                .Include(s => s.Component.Department.Employees)
                .Include(s => s.StudyPlan.Groups)
                .Select(s => new
                {
                    s.Id,
                    Employees = s.Component.Department.Employees.Select(e => e.Id).ToList(),
                    Groups = s.StudyPlan.Groups.Select(g => g.Id).ToList()
                })
                .ToList();

            sw.Stop();

            Console.WriteLine($"Query {subjects.Count} subjects with groups and employees. Execution: {sw.ElapsedMilliseconds} ms.");

            sw.Restart();

            var reports = new List<MarkReport>();

            var days = (DateTime.Today - new DateTime(2010, 1, 1)).Days;

            foreach (var subject in subjects)
            {
                foreach (var group in subject.Groups) 
                {
                    var reportsCount = Random.Shared.Next(3, 16);

                    for (int i = 0; i < reportsCount; i++) 
                    {
                        var report = new MarkReport
                        {
                            GroupId = group,
                            SubjectId = subject.Id,
                            EmployeeId = subject.Employees[Random.Shared.Next(subject.Employees.Count)],
                            Date = DateTime.Today.AddDays(-1 * Random.Shared.Next(days))
                        };

                        reports.Add(report);
                    }
                }
            }
            sw.Stop();

            Console.WriteLine($"Generated {reports.Count} MarkReports. Execution: {sw.ElapsedMilliseconds} ms.");

            sw.Restart();

            _ctx.BulkInsert(reports);

            sw.Stop();

            Console.WriteLine($"Inserted {reports.Count} MarkReports. Execution: {sw.ElapsedMilliseconds} ms.");
        }
    }
}
