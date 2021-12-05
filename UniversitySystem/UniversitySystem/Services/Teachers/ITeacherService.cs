namespace UniversitySystem.Services.Teachers
{
    using System.Collections.Generic;

    using UniversitySystem.Services.Teachers.Models;

    public interface ITeacherService
    {
        public void Create(string firstName,
            string lastName,
            int facultyId,
            int titleId);

        public ICollection<TitleServiceModel> GetTitles();

    }
}
