namespace TestDataGenerator.Models
{
    public enum SubjectTypes
    {
        MainCommon = 1,
        MainProfessional,
    }

    public class Subject
    {
        public int Id { get; set; }
        public int ComponentId { get; set; }
        public int Semester { get; set; }
        public int LectionHours { get; set; }
        public int PracticHours { get; set; }
        public int LabourHours { get; set; }
        public int SelfHours { get; set; }
        public RgrTypes RGR { get; set; }
        public CwTypes CW { get; set; }
        public GradingTypes GradingType { get; set; }
        public int StudyPlanId { get; set; }
        public SubjectTypes SubjectType { get; set; }

        public Component Component { get; set; }
        public StudyPlan StudyPlan { get; set; }
    }
}
