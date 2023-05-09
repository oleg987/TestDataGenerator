namespace TestDataGenerator.Models
{
    public class Department
    {
        public int Id { get; set; }
        public int DepartmentTypeId { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
