namespace TestDataGenerator.Models
{
    public class Department
    {
        public int Id { get; set; }
        public int DepartmentTypeId { get; set; }
        public int? ParentId { get; set; }
        public string? Abbr { get; set; }
        public ICollection<Employee>? Employees { get; set; }
        public Department? Parent { get; set; }

    }
}
