using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TestDataGenerator.Models;

namespace TestDataGenerator.Generators
{
    public class MarkGenerator : GeneratorBase
    {
        public MarkGenerator(string connectionString) : base(connectionString)
        {

        }

        public void Generate()
        {
            var sw = new Stopwatch();
            sw.Start();

            var markReports = _ctx.MarkReport
                .AsSplitQuery()
                .Where(m => m.Group.Students.Count > 1)
                .Select(m => new
                {
                    m.Id,
                    Students = m.Group.Students.Select(s => s.Id).ToList()
                })
                .ToList();

            sw.Stop();

            Console.WriteLine($"Query {markReports.Count} mark reports with students. Execution: {sw.ElapsedMilliseconds} ms.");

            sw.Restart();

            var marks = new List<Mark>();

            var counter = 0;

            foreach (var report in markReports)
            {
                ++counter;

                int marksInReport = Random.Shared.Next(1, report.Students.Count);            

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

            sw.Stop();

            Console.WriteLine($"Generated and inserted {counter} marks. Execution: {sw.ElapsedMilliseconds} ms.");
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
