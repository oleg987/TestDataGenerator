using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGenerator.Models
{
    public class StudyFlow
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<EmployeeSubject> EmployeeSubjects { get; set; }
    }
}
