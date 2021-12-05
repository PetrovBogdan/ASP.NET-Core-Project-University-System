namespace UniversitySystem.Services.Statistics
{
    using System.Collections.Generic;
    using UniversitySystem.Services.Statistics.Models;

    public interface IStatisticsService
    {
        public IDictionary<string, List<PersonListingServiceModel>> GetPeople();

        public ICollection<StudentCourseServiceModel> GetStudentsWithCourses();

        public ICollection<StudentCreditsServiceModel> GetStudentsWithCredits();

        public ICollection<TeacherCourseStudentsServiceModel> GetTeachersWithCoursesAndStudents();

        public ICollection<CourseStatisticsServiceModel> GetTopCourses();

        public ICollection<TeacherServiceModel> GetTopTeachers();
    }
}
