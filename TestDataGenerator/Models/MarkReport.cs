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
    }
}
