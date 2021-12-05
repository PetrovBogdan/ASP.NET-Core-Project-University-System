namespace UniversitySystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using UniversitySystem.Services.Courses.Models;

    public class CourseFormModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Credit { get; set; }

        public ICollection<TeacherServiceModel> Teachers { get; set; }

        [Required(ErrorMessage = "You must choose at least one teacher for the course you are adding.")]
        public List<int> TeacherId { get; set; }

        public ICollection<string> Courses { get; set; }

    }
}
