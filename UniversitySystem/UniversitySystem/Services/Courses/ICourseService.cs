namespace UniversitySystem.Services.Courses
{
    using System.Collections.Generic;
    using UniversitySystem.Services.Courses.Models;
    using UniversitySystem.Services.Students.Models;

    public interface ICourseService
    {
        public ICollection<string> GetExistingCourses(int facultyId);

        public ICollection<TeacherServiceModel> GetTeachers(int facultyId);

        public void Create(string name, 
            int credit, 
            int facultyId,
            ICollection<int> teachers);
    }
}
