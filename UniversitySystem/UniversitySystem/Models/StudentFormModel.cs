namespace UniversitySystem.Models
{
    using System.Collections.Generic;
    using UniversitySystem.Services.Students.Models;

    public class StudentFormModel : PersonFormModel
    {
        public ICollection<CourseServiceModel> Courses { get; set; }

        public ICollection<int?> CourseId { get; set; }
    }
}
