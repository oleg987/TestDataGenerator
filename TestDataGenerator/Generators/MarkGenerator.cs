using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataGenerator.Models;

namespace TestDataGenerator.Generators
{
    public class MarkGenerator
    {
        private readonly IsDbContext _ctx;

        public MarkGenerator()
        {
            _ctx = new IsDbContext();
        }

        public void Generate()
        {
            var markReports = _ctx.MarkReport
                .AsSplitQuery()
                .AsNoTracking()
                .Include(m => m.Group.Students)
                .Select(m => new
                {
                    m.Id,
                    Students = m.Group.Students.Select(s => s.Id).ToList()
                })
                .ToList();

            var marks = new List<Mark>();

            var counter = 0;

            foreach (var report in markReports)
            {
                ++counter;
                var marksInReport = Random.Shared.Next(1, report.Students.Count);

                HashSet<int> students = GetRandomStudents(report.Students, marksInReport);

                foreach (var student in students)
                {
                    var mark = new Mark
                    {
                        StudentId = student,
                        MarkReportId = report.Id,
                        Value = Random.Shared.Next(1, 101)
                    };

                    marks.Add(mark);
                }

                if (counter % 1000 == 0)
                {
                    _ctx.BulkInsert(marks);
                    marks.Clear();
                }
            }

            _ctx.BulkInsert(marks);
        }

        private static HashSet<int> GetRandomStudents(List<int> students, int marksInReport)
        {
            var result = new HashSet<int>(marksInReport);

            do
            {
                result.Add(students[Random.Shared.Next(students.Count)]);
            } while (result.Count != marksInReport);

            return result;
        }
    }
}
