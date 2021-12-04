namespace UniversitySystem.Services.Statistics.Models
{
    using System.Collections.Generic;

    public class StudentCourseServiceModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public ICollection<string> Courses { get; set; } = new List<string>();

    }
}
