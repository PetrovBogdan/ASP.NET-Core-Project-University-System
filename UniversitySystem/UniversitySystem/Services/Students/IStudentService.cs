namespace UniversitySystem.Services.Students
{
    using System.Collections.Generic;
    using UniversitySystem.Services.Students.Models;

    public interface IStudentService
    {
        public void Create(string firstName,
            string lastName,
            int facultyId,
            ICollection<int?> courses);

        public ICollection<CourseServiceModel> GetCourses();

    }
}
