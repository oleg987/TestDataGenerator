using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGenerator.Models
{
    public class MarkReport
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int GroupId { get; set; }
        public int SubjectId { get; set; }
        public int EmployeeId { get; set; }
        public string? ReportCode { get; set; }

        public Group Group { get; set; }
        public Subject Subject { get; set; }
        public Employee Employee { get; set; }
        public ICollection<Mark> Marks { get; set; }
    }
}
