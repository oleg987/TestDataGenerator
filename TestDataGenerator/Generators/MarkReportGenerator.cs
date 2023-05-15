using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TestDataGenerator.Models;

namespace TestDataGenerator.Generators
{
    public class MarkReportGenerator : GeneratorBase
    {
        public MarkReportGenerator(string connectionString) : base(connectionString)
        {
            
        }

        public void Generate()
        {
            var sw = new Stopwatch();
            sw.Start();

            var subjects = _ctx.Subjects
                .AsSplitQuery()
                .Select(s => new
                {
                    s.Id,
                    Employees = s.Component.Department.Employees.Select(e => e.Id).ToList(),
                    Groups = s.StudyPlan.Groups.Select(g => g.Id).ToList(),
                    InstituteAbbr = s.Component.Department.Parent.Abbr ?? s.Component.Department.Abbr
                })
                .ToList();

            sw.Stop();

            Console.WriteLine($"Query {subjects.Count} subjects with groups and employees. Execution: {sw.ElapsedMilliseconds} ms.");

            sw.Restart();

            var reports = new List<MarkReport>();

            var days = (DateTime.Today - new DateTime(2010, 1, 1)).Days;

            var numbers = subjects
                .Select(s => s.InstituteAbbr)
                .Distinct()
                .ToDictionary(k => k, v => Enumerable.Range(2010, 14).ToDictionary(k => k, v => 0));

            foreach (var subject in subjects)
            {
                foreach (var group in subject.Groups) 
                {
                    var reportsCount = Random.Shared.Next(3, 16);

                    for (int i = 0; i < reportsCount; i++) 
                    {
                        var date = DateTime.Today.AddDays(-1 * Random.Shared.Next(days));

                        var report = new MarkReport
                        {
                            GroupId = group,
                            SubjectId = subject.Id,
                            EmployeeId = subject.Employees[Random.Shared.Next(subject.Employees.Count)],
                            Date = date,
                            ReportCode = $"{subject.InstituteAbbr}-{++numbers[subject.InstituteAbbr][date.Year]}/{date.Year}"
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
