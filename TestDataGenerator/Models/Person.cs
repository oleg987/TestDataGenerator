using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGenerator.Models
{
    public class Person
    {
        public int Id { get; set; }
        public int? EdboId { get; set; }
        public string? PersonCode { get; set; }
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
    }
}
