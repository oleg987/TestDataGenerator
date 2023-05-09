using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGenerator.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int PersonId { get; set; }

        public Person Person { get; set; }
        public Department Department { get; set; }
        public ICollection<Component> Components { get; set; }
        public ICollection<EmployeeSubject> EmployeeSubjects { get; set; }
        public ICollection<MarkReport> MarkReports { get; set; }
    }
}
