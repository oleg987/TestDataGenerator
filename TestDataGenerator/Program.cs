using Bogus;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Text.Json;
using TestDataGenerator.Generators;

namespace TestDataGenerator
{

    internal class Program
    {


        private const string ALLOWED_CHARS = "QWERTYUIOPASDFGHJKLZXCVBNM";

        static void Main(string[] args)
        {
            //var persons = GeneratePersons(6000);

            //var groups = GenerateGroups(6000);

            //var studentGenerator = new StudentGenerator();
            //studentGenerator.Generate(6000);

            var titles = GetComponentTitles();
        }

        static List<string> GetComponentTitles()
        {
            using var file = new FileStream("titles.txt", FileMode.Open);
            using var reader = new StreamReader(file);

            var content = reader.ReadToEnd();
            Console.WriteLine(content);

            return JsonSerializer.Deserialize<List<string>>(content) ?? new List<string>();
        }

        /*
         * Group title pattern: LL-NNN (L - letter, N - number) !
         * Group title unique
         * Groups count: students in group 5 - 30
         * Student can`t be presented in more than one group
         * All students must be with group
         */
        static Dictionary<int, int> GenerateGroups(int studentsCount) // what we need to return??? !
        {
            var titles = new HashSet<string>();

            var groups = new List<Models.Group>();

            var dict = new Dictionary<Models.Group, int>();

            var studentsInCurrentGroup = Random.Shared.Next(5, 31);

            var remainigStudents = studentsCount - studentsInCurrentGroup;

            if (remainigStudents >= 0)
            {
                studentsCount = remainigStudents;
            }
            else
            {
                studentsInCurrentGroup = studentsCount;
            }

            string? title = null;

            while (true)
            {
                title = GenerateGroupTitle();

                if (titles.Add(title)) break;
            }

            var group = new Models.Group() { Title = title };
            groups.Add(group);            

            dict.Add(group, studentsInCurrentGroup);

            // save to DB.

            return dict.ToDictionary(k => k.Key.Id, v => v.Value);
        }

        static string GenerateGroupTitle()
        {
            return $"{ALLOWED_CHARS[Random.Shared.Next(ALLOWED_CHARS.Length)]}{ALLOWED_CHARS[Random.Shared.Next(ALLOWED_CHARS.Length)]}-{Random.Shared.Next(100, 1000)}";
        }

        static int[] GeneratePersons(int count)
        {
            using var ctx = new IsDbContext();

            var countries = ctx.Countries
                .AsNoTracking()
                .Where(c => c.Id > 0)
                .Select(c => c.Id)
                .ToArray();

            var fakePersons = new Faker<Models.Person>("uk")
                .RuleFor(p => p.Id, f => 0)
                .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                .RuleFor(p => p.LastName, f => f.Person.LastName)
                .RuleFor(p => p.FirstNameEn, (f, p) => p.FirstName)
                .RuleFor(p => p.LastNameEn, (f, p) => p.LastName)
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