using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGenerator.Generators
{
    public class SubjectGenerator
    {
        private readonly IsDbContext _ctx;

        public SubjectGenerator()
        {
            _ctx = new IsDbContext();
        }



        public void Generate()
        {
            var subjects = new List<Models.Subject>();

            var studyPlanIds = _ctx.StudyPlans
                .AsNoTracking()
                .Select(s => s.Id)
                .ToList();

            var components = _ctx.Components
                .AsNoTracking()
                .ToList();

            foreach (var studyPlanId in studyPlanIds)
            {
                var componentsCount = Random.Shared.Next(20, 31);

                var planComponents = GetRandomUniqueNameComponents(componentsCount, components);

                foreach (var component in planComponents)
                {
                    var semestersCount = 0;

                    if (component.CW != Models.CwTypes.None && component.RGR != Models.RgrTypes.None)
                    {
                        semestersCount = Random.Shared.Next(2, 9);
                    }
                    else
                    {
                        semestersCount = Random.Shared.Next(1, 9);
                    }

                    var semesterNums = GenerateSemesterNums(semestersCount);

                    var componentSubjects = new List<Models.Subject>();

                    var hours = GenerateHours(component, semesterNums.ToList());

                    foreach (var semester in semesterNums)
                    {
                        var subject = new Models.Subject
                        {
                            ComponentId = component.Id,
                            Semester = semester,
                            GradingType = component.GradingType == Models.GradingTypes.Offset ? Models.GradingTypes.Offset : (Models.GradingTypes)Random.Shared.Next(1, 3),
                            StudyPlanId = studyPlanId,
                            SubjectType = (Models.SubjectTypes)Random.Shared.Next(1, 3),
                            LectionHours = hours[semester][0],
                            PracticHours = hours[semester][1],
                            LabourHours = hours[semester][2],
                            SelfHours = hours[semester][3]
                        };

                        componentSubjects.Add(subject);
                    }

                    // check exam
                    if (component.GradingType == Models.GradingTypes.Exam && !componentSubjects.Any(s => s.GradingType == Models.GradingTypes.Exam))
                    {
                        componentSubjects[Random.Shared.Next(componentSubjects.Count)].GradingType = Models.GradingTypes.Exam;
                    }

                    // check CW
                    if (component.CW != Models.CwTypes.None)
                    {
                        componentSubjects[Random.Shared.Next(componentSubjects.Count)].CW = Models.CwTypes.CW;
                    }

                    // check RGR
                    if (component.RGR != Models.RgrTypes.None)
                    {
                        componentSubjects.Where(c => c.CW != Models.CwTypes.CW).Last().RGR = component.RGR;
                    }

                    subjects.AddRange(componentSubjects);
                }
            }

            _ctx.Subjects.AddRange(subjects);

            _ctx.BulkSaveChanges();
        }

        private static HashSet<int> GenerateSemesterNums(int semestersCount)
        {
            var semesterNums = new HashSet<int>(semestersCount);

            while(semesterNums.Count < semestersCount) 
            {
                semesterNums.Add(Random.Shared.Next(1, 9));
            }

            return semesterNums;
        }

        private static Dictionary<int, List<int>> GenerateHours(Models.Component component, List<int> semesterNums)
        {
            var result = new Dictionary<int, List<int>>(semesterNums.Count);

            var lection = component.LectionHours;
            var practic = component.PracticHours;
            var labour = component.LabourHours;
            var self = component.SelfHours;

            for (int i = 0; i < semesterNums.Count - 1; i++)
            {
                var hours = new List<int>(4);

                var currentLection = Random.Shared.Next(0, lection);
                hours.Add(currentLection);
                lection -= currentLection;

                var currentPractic = Random.Shared.Next(0, practic);
                hours.Add(currentPractic);
                practic -= currentPractic;

                var currentLabour = Random.Shared.Next(0, labour);
                hours.Add(currentLabour);
                labour -= currentLabour;

                var currentSelf = Random.Shared.Next(0, self);
                hours.Add(currentSelf);
                self -= currentSelf;

                result[semesterNums[i]] = hours;
            }

            result[semesterNums.Last()] = new List<int> { lection, practic, labour, self };

            return result.Any(r => r.Value.Sum() == 0) ? GenerateHours(component, semesterNums) : result;
        }

        private static HashSet<Models.Component> GetRandomUniqueNameComponents(int componentsCount, List<Models.Component> components)
        {
            var result = new HashSet<Models.Component>(componentsCount, new ComponentUniqueTitleComparer());

            while (result.Count < componentsCount) 
            {
                var component = components[Random.Shared.Next(components.Count)];

                result.Add(component);
            }

            return result;
        }
    }
}
