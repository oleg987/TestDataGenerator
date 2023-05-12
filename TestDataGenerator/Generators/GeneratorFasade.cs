namespace TestDataGenerator.Generators
{
    public class GeneratorFasade
    {
        private readonly string _connectionString;

        public GeneratorFasade(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Generate()
        {
            using (var studentGenerator = new StudentGenerator(_connectionString))
            {
                studentGenerator.Generate(10_000);
            }

            using (var componentGenerator = new ComponentGenerator(_connectionString))
            {
                componentGenerator.Generate(1_000);
            }

            using (var subjectGenerator = new SubjectGenerator(_connectionString))
            {
                subjectGenerator.Generate();
            }

            using (var markReportGenerator = new AlternativeMarkReportGenerator(_connectionString))
            {
                markReportGenerator.Generate();
            }

            using (var markGenerator = new MarkGenerator(_connectionString))
            {
                markGenerator.Generate();
            }
        }
    }
}
