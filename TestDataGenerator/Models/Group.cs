using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGenerator.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int? StudyPlanId { get; set; }
        public StudyPlan? StudyPlan { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<StudyFlow> StudyFlows { get; set; }
    }
}
