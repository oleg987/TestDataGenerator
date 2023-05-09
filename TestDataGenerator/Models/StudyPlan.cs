using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGenerator.Models
{
    public class StudyPlan
    {
        public int Id { get; set; }
        public int? DepartmentId { get; set; }

        public Department? Department { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<Subject> Subjects { get; set; }
    }
}
