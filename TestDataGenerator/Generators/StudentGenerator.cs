using Bogus;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TestDataGenerator.Models;

namespace TestDataGenerator.Generators
{
    public class StudentGenerator : GeneratorBase
    {
        private const string ALLOWED_CHARS = "QWERTYUIOPASDFGHJKLZXCVBNM";
        public StudentGenerator(string connectionString) : base(connectionString)
        {

        }

        public void Generate(int count)
        {
            var sw = new Stopwatch();

            sw.Start();

            var personIds = GeneratePersons(count);

            sw.Stop();

            Console.WriteLine($"Inserted {count} persons. Execution: {sw.ElapsedMilliseconds} ms.");

            sw.Restart();

            var groupsWithStudentCount = GenerateGroups(count);

            sw.Stop();

            Console.WriteLine($"Inserted {groupsWithStudentCount.Count} groups. Execution: {sw.ElapsedMilliseconds} ms.");

            sw.Restart();
            GenerateStudents(personIds, groupsWithStudentCount);
            sw.Stop();
            Console.WriteLine($"Inserted {count} students. Execution: {sw.ElapsedMilliseconds} ms.");
        }

        private void GenerateStudents(int[] personIds, Dictionary<int, int> groupsWithStudentCount)
        {
            var students = new List<Student>(personIds.Length);

            var currentStudent = 0;

            var currentStudentId = _ctx.Students.Any() ? _ctx.Students.Max(s => s.Id) : 0;

            foreach (var group in groupsWithStudentCount)
            {
                for (int i = 0; i < group.Value; i++)
                {
                    var student = new Student
                    {
                        Id = ++currentStudentId,
                        PersonId = personIds[currentStudent++],
                        Begin = DateTime.Now,
                        End = DateTime.Now,
                        IsDualForm = Random.Shared.Next(2) == 1,
                        IsSecondHigher = Random.Shared.Next(2) == 1,
                        EduFinanceId = Random.Shared.Next(1, 3),
                        GroupId = group.Key,
                        Course = 0
                    };

                    students.Add(student);
                }
            }

            _ctx.BulkInsert(students);
        }

        private Dictionary<int, int> GenerateGroups(int studentsCount)
        {
            var groupTitles = new HashSet<string>();
            var groups = new List<Group>();
            var groupsWithStudentCount = new Dictionary<Group, int>();

            var studyPlanIds = _ctx.StudyPlans
                .AsNoTracking()
                .Select(s => s.Id)
                .ToArray();

            do
            {
                var countOfStudentsInGroup = Random.Shared.Next(5, 31);

                var remainingStudents = studentsCount - countOfStudentsInGroup;

                if (remainingStudents <= 0)
                {
                    countOfStudentsInGroup = studentsCount;
                    studentsCount = 0;
                }
                else
                {
                    studentsCount = remainingStudents;
                }

                string title = string.Empty;
                while (true)
                {
                    title = GenerateGroupTitle();
                    if (groupTitles.Add(title)) break;
                }

                var group = new Group { Title = title, StudyPlanId = studyPlanIds[Random.Shared.Next(studyPlanIds.Length)] };
                groups.Add(group);
                groupsWithStudentCount.Add(group, countOfStudentsInGroup);
            } while (studentsCount > 0);

            _ctx.Groups.AddRange(groups);

            _ctx.BulkSaveChanges();

            return groupsWithStudentCount.ToDictionary(k => k.Key.Id, v => v.Value);
        }

        static string GenerateGroupTitle()
        {
            return $"{ALLOWED_CHARS[Random.Shared.Next(ALLOWED_CHARS.Length)]}{ALLOWED_CHARS[Random.Shared.Next(ALLOWED_CHARS.Length)]}-{Random.Shared.Next(100, 1000)}";
        }
        private int[] GeneratePersons(int count)
        {
            var countries = _ctx.Countries
                .AsNoTracking()
                .Where(c => c.Id > 0)
                .Select(c => c.Id)
                .ToArray();

            var fakePersons = new Faker<Models.Person>("uk")
                .RuleFor(p => p.Id, f => 0)
                .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                .RuleFor(p => p.LastName, f => f.Person.LastName)
                .RuleFor(p => p.FirstNameEn, (f, p) => Transliterater.Translate(p.FirstName, "uk"))
                .RuleFor(p => p.LastNameEn, (f, p) => Transliterater.Translate(p.LastName, "uk"))
                .RuleFor(p => p.Birthday, f => f.Date.Between(new DateTime(1995, 1, 1), new DateTime(2005, 1, 1)))
                .RuleFor(p => p.Gender, f => f.Random.Int(1, 2)) // include lower and upper bound
                .RuleFor(p => p.IsUkr, f => true)
                .RuleFor(p => p.CountryId, f => countries[Random.Shared.Next(countries.Length)]);

            var persons = fakePersons.Generate(count);

            _ctx.Persons.AddRange(persons);

            _ctx.BulkSaveChanges();

            return persons.Select(p => p.Id).ToArray();
        }
    }
}
