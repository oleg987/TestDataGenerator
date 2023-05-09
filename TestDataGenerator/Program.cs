using Bogus;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace TestDataGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var persons = GeneratePersons(6000);

            File.WriteAllText("person_ids.txt", JsonSerializer.Serialize(persons));
        }

        static List<int> GenerateGroups(int studentsCount) // what we need to return???
        {
            /*
             * Group title pattern: LL-NNN (L - letter, N - number)
             * Group title unique
             * Groups count: students in group 5 - 30
             * Student can`t be presented in more than one group
             * All students must be with group
             */
            return null;
        }

        static List<int> GeneratePersons(int count)
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

            return persons.Select(p => p.Id).ToList();
        }
    }
}