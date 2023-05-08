using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGenerator.Models
{
    public class Department
    {
        public int Id { get; set; }
        public int DepartmentTypeId { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
