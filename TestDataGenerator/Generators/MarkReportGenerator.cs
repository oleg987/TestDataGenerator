using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
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
            var markReports = new List<MarkReport>();

            var studyPlans = _ctx.StudyPlans
                .AsNoTracking()
                .AsSplitQuery()
                .Where(s => s.Groups.Any())
                .Include(s => s.Groups)
                .Include(s => s.Subjects)
                    .ThenInclude(s => s.Component)
                        .ThenInclude(c => c.Department)
                            .ThenInclude(d => d.Employees)
                .Select(s => new
                {
                    Groups = s.Groups.Select(g => g.Id),
                    Subjects = s.Subjects.Select(sub => new
                    {
                        sub.Id,
                        Employees = sub.Component.Department.Employees.Select(e => e.Id).ToList(),
                    })
                })
                .ToList();

            foreach (var studyPlan in studyPlans)
            {
                foreach (var group in studyPlan.Groups)
                {
                    foreach (var subjects in studyPlan.Subjects)
                    {
                        var markReport = new MarkReport
                        {
                            Date = new DateTime(Random.Shared.Next(2010, 2024), Random.Shared.Next(1, 13), Random.Shared.Next(1, 29)),
                            GroupId = group,
                            SubjectId = subjects.Id,
                            EmployeeId = subjects.Employees[Random.Shared.Next(subjects.Employees.Count)],
                            ReportCode = GenerateReportCode()
                        };

                        markReports.Add(markReport);
                    }
                }
            }

            _ctx.MarkReport.AddRange(markReports);

            _ctx.BulkSaveChanges();
        }

        private static string GenerateReportCode()
        {
            return null;
        }
    }
}
