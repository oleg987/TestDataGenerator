using System.Diagnostics;
using TestDataGenerator.Generators;

namespace TestDataGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            string _connectionString = string.Empty;

            using (var file = new FileStream("connection.txt", FileMode.Open))
            {
                using (var reader = new StreamReader(file))
                {
                    _connectionString = reader.ReadToEnd().Trim();
                }
            }

            using (var markGenerator = new MarkGenerator(_connectionString))
            {
                markGenerator.Generate();
            }

            sw.Stop();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Total generation time {sw.ElapsedMilliseconds} ms.");
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}
