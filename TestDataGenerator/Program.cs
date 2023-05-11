using TestDataGenerator.Generators;

namespace TestDataGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var studentGenerator = new StudentGenerator();
            //studentGenerator.Generate(10000);

            // Generate 1000 components. 
            // Unique: set of (Title, Hours, GradingType, Cw, RGR)
            //var componentsGenerator = new ComponentGenerator();
            //componentsGenerator.Generate(1000);

            //// Generate subjects
            //var subjectsGenerator = new SubjectGenerator();
            //subjectsGenerator.Generate();

            // Generate MarkReports
            //var markReportGenerator = new AlternativeMarkReportGenerator();
            //markReportGenerator.Generate();

            var markGenerator = new MarkGenerator();
            markGenerator.Generate();
        }
    }
}
