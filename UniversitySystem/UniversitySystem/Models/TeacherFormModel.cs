namespace UniversitySystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using UniversitySystem.Services.Teachers.Models;

    public class TeacherFormModel : PersonFormModel
    {
        public ICollection<TitleServiceModel> Titles { get; set; }

        [Required(ErrorMessage ="You must select title for the teacher.")]
        public int TitleId { get; set; }
    }
}
