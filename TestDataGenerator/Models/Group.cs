namespace TestDataGenerator.Models
{
    public class Group : IEquatable<Group>
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int? StudyPlanId { get; set; }
        public StudyPlan? StudyPlan { get; set; }
        public ICollection<Student> Students { get; set; }

        public bool Equals(Group? other)
        {
            return other is not null && other.Title == Title;
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
    }
}
