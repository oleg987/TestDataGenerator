using System.Text.Json;

namespace TestDataGenerator.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string FirstNameEn { get; set; }
        public string LastNameEn { get; set; }
        public string? MiddleNameEn { get; set; }
        public DateTime Birthday { get; set; }
        public int CountryId { get; set; }
        public int Gender { get; set; }
        public bool IsUkr { get; set; }

        public override string? ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
