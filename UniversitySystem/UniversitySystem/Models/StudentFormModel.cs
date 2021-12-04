namespace UniversitySystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using UniversitySystem.Services.Students.Models;

    public class StudentFormModel : PersonFormModel
    {
        public ICollection<CourseServiceModel> Courses { get; set; }

        [Required(ErrorMessage = "You must select a course !")]
        public ICollection<int?> CourseId { get; set; }
    }
}
