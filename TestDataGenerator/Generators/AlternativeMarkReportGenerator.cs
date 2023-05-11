using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using TestDataGenerator.Models;

namespace TestDataGenerator.Generators
{
    public class AlternativeMarkReportGenerator
    {
        private readonly IsDbContext _ctx;

        public AlternativeMarkReportGenerator()
        {
            _ctx = new IsDbContext();
        }

        public void Generate()
        {
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

            _ctx.BulkInsert(reports);
        }
    }
}
