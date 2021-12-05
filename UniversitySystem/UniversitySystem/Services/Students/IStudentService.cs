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

        public ICollection<CourseServiceModel> GetCourses(int facultyId);

        public StudentDetailsServiceModel GetDetails(int studentId);

        public void AddCourse(int courseId, int studentId);

        public void RemoveCourse(int courseId, int studentId);

    }
}
