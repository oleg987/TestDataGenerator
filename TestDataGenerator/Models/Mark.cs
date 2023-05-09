using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGenerator.Models
{
    public class Mark
    {
        public int StudentId { get; set; }
        public int MarkReportId { get; set; }
        public int Value { get; set; }

        public Student Student { get; set; }
        public MarkReport MarkReport { get; set; }
    }
}
