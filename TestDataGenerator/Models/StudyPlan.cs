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
