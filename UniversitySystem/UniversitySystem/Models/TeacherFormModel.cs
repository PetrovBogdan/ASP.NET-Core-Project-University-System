namespace UniversitySystem.Models
{
    using System.Collections.Generic;
    using UniversitySystem.Services.Teachers.Models;

    public class TeacherFormModel : PersonFormModel
    {
        public ICollection<TitleServiceModel> Titles { get; set; }

        public ICollection<int?> TitleId { get; set; }
    }
}
