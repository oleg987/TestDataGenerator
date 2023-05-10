using Bogus;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace TestDataGenerator.Generators
{
    public class StudentGenerator
    {
        private const string ALLOWED_CHARS = "QWERTYUIOPASDFGHJKLZXCVBNM";
        private readonly IsDbContext ctx;

        public StudentGenerator()
        {
            ctx = new IsDbContext();
        }

        public void Generate(int count)
        {
            var personIds = GeneratePersons(count);

            var groupsWithStudentCount = GenerateGroups(count);

            GenerateStudents(personIds, groupsWithStudentCount);            
        }

        private void GenerateStudents(int[] personIds, Dictionary<int, int> groupsWithStudentCount)
        {
            var students = new List<Models.Student>(personIds.Length);

            var currentStudent = 0;

            var currentStudentId = ctx.Students.Any() ? ctx.Students.Max(s => s.Id) : 0;

            foreach (var group in groupsWithStudentCount)
            {
                for (int i = 0; i < group.Value; i++)
                {
                    var student = new Models.Student
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

            ctx.Students.AddRange(students);

            ctx.BulkSaveChanges();
        }

        private Dictionary<int, int> GenerateGroups(int studentsCount)
        {
            var groupTitles = new HashSet<string>();
            var groups = new List<Models.Group>();
            var groupsWithStudentCount = new Dictionary<Models.Group, int>();

            var studyPlanIds = ctx.StudyPlans
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

                var group = new Models.Group { Title = title, StudyPlanId = studyPlanIds[Random.Shared.Next(studyPlanIds.Length)] };
                groups.Add(group);
                groupsWithStudentCount.Add(group, countOfStudentsInGroup);
            } while (studentsCount > 0);

            ctx.Groups.AddRange(groups);

            ctx.BulkSaveChanges();

            return groupsWithStudentCount.ToDictionary(k => k.Key.Id, v => v.Value);
        }

        static string GenerateGroupTitle()
        {
            return $"{ALLOWED_CHARS[Random.Shared.Next(ALLOWED_CHARS.Length)]}{ALLOWED_CHARS[Random.Shared.Next(ALLOWED_CHARS.Length)]}-{Random.Shared.Next(100, 1000)}";
        }
        private int[] GeneratePersons(int count)
        {
            var countries = ctx.Countries
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

            ctx.Persons.AddRange(persons);

            ctx.BulkSaveChanges();

            return persons.Select(p => p.Id).ToArray();
        }
    }
}
