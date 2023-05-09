namespace TestDataGenerator.Models
{
    public class StudyFlow
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}
