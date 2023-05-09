using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGenerator.Models
{
    public class EmployeeSubject
    {
        public int SubjectId { get; set; }
        public int EmployeeId { get; set; }
        public int StudyFlowId { get; set; }
        public int LectionHours { get; set; }
        public int PracticHours { get; set; }
        public int LabourHours { get; set; }
        public int SelfHours { get; set; }
        public Subject Subject { get; set; }
        public Employee Employee { get; set; }
        public StudyFlow StudyFlow { get; set; }
    }
}
