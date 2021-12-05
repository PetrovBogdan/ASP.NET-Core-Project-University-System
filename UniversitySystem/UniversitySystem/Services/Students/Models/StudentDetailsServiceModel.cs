namespace UniversitySystem.Services.Students.Models
{
    using System.Collections.Generic;
    public class StudentDetailsServiceModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<CourseServiceModel> Courses { get; set; } = new List<CourseServiceModel>();

        public ICollection<CourseServiceModel> ActiveCourses { get; set; } = new List<CourseServiceModel>();    

    }
}
