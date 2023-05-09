namespace TestDataGenerator.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }

        public Department Department { get; set; }
    }
}
