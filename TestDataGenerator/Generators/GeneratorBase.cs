namespace TestDataGenerator.Generators
{
    public abstract class GeneratorBase : IDisposable
    {
        protected readonly IsDbContext _ctx;

        protected GeneratorBase(string connectionString)
        {
            _ctx = new IsDbContext(connectionString);
        }

        public void Dispose() => _ctx.Dispose();
    }
}
