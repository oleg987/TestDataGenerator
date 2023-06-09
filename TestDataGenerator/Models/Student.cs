﻿namespace TestDataGenerator.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public bool IsDualForm { get; set; }
        public bool IsSecondHigher { get; set; }
        public int EduFinanceId { get; set; }
        public int? GroupId { get; set; }
        public int Course { get; set; }
    }
}
