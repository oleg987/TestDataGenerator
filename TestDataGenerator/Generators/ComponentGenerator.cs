using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Noding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestDataGenerator.Models;

namespace TestDataGenerator.Generators
{
    public class ComponentGenerator
    {
        private readonly IsDbContext _ctx;

        public ComponentGenerator()
        {
            _ctx = new IsDbContext();
        }

        public int[] Generate(int count)
        {
            var components = new HashSet<Models.Component>(count);

            var titles = GetComponentTitles();

            // Проверка на наличие сотрудников
            var departments = _ctx.Departments
                .AsNoTracking()
                .Where(d => d.DepartmentTypeId == 30 && d.Employees.Any())
                .Select(d => d.Id)
                .ToList();

            while (components.Count < 1000)
            {
                var hours = GenerateHours();

                var title = titles[Random.Shared.Next(titles.Count)];

                var component = new Models.Component()
                {
                    TitleEn = title,
                    TitleUa = title,
                    RGR = (RgrTypes)Random.Shared.Next(0, 3),
                    CW = (CwTypes)Random.Shared.Next(0, 2),
                    GradingType = (GradingTypes)Random.Shared.Next(1, 3),
                    Year = Random.Shared.Next(2010, 2024),
                    CanBeReleased = false,
                    DepartmentId = departments[Random.Shared.Next(departments.Count)],
                    ComponentType = ComponentType.Subject,
                    LectionHours = hours[0],
                    PracticHours = hours[1],
                    LabourHours = hours[2],
                    SelfHours = hours[3]
                };

                components.Add(component);
            }

            _ctx.Components.AddRange(components);

            _ctx.BulkSaveChanges();

            return components.Select(d => d.Id).ToArray();
        }

        private static List<string> GetComponentTitles()
        {
            using var file = new FileStream("titles.txt", FileMode.Open);
            using var reader = new StreamReader(file);

            var content = reader.ReadToEnd();

            return JsonSerializer.Deserialize<List<string>>(content) ?? new List<string>();
        }

        private static List<int> GenerateHours()
        {
            var hours = new List<int>(4);

            for (int i = 0; i < hours.Capacity; i++)
            {
                hours.Add(Random.Shared.Next(0, 28) * 15);
            }

            return hours.All(h => h == 0) ? GenerateHours() : hours;
        }
    }
}
