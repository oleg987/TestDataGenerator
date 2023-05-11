namespace TestDataGenerator.Models
{
    public enum ComponentType
    {
        Subject = 1,
        Attestation,
        Practic,
        CourseProject
    }

    public enum GradingTypes
    {
        Exam = 1,
        Offset
    }

    public enum RgrTypes
    {
        None = 0,
        RGR,
        RR
    }

    public enum CwTypes
    {
        None = 0,
        CW
    }

    public class Component : IEquatable<Component>
    {
        public int Id { get; set; }
        public string TitleUa { get; set; }
        public string TitleEn { get; set; }
        public int LectionHours { get; set; }
        public int PracticHours { get; set; }
        public int LabourHours { get; set; }
        public int SelfHours { get; set; }
        public RgrTypes RGR { get; set; }
        public CwTypes CW { get; set; }
        public int Year { get; set; }
        public bool CanBeReleased { get; set; }
        public int DepartmentId { get; set; }
        public GradingTypes GradingType { get; set; }
        public ComponentType ComponentType { get; set; }

        public Department Department { get; set; }
        public ICollection<Subject> Subjects { get; set; }

        public bool Equals(Component? other)
        {
            return other is not null && other.TitleUa == TitleUa
                                    && other.LectionHours == LectionHours
                                    && other.PracticHours == PracticHours
                                    && other.LabourHours == LabourHours
                                    && other.SelfHours == SelfHours
                                    && other.RGR == RGR
                                    && other.CW == CW
                                    && other.GradingType == GradingType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TitleUa, LectionHours, PracticHours, LabourHours, SelfHours, RGR, CW, GradingType);
        }
    }
}
