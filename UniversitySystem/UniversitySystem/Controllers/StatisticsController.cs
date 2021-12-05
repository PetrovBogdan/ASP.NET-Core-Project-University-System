namespace UniversitySystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using UniversitySystem.Services.Statistics;

    public class StatisticsController : Controller
    {
        private readonly IStatisticsService statistics;

        public StatisticsController(IStatisticsService statistics)
            => this.statistics = statistics;

        public IActionResult All()
            => View(this.statistics
                             .GetPeople());

        public IActionResult StudentsWithCourses()
            => View(this.statistics
                            .GetStudentsWithCourses());

        public IActionResult StudentsWithCredits()
            => View(this.statistics
                            .GetStudentsWithCredits());

        public IActionResult TeachersWithCoursesAndStudents()
            => View(this.statistics
                            .GetTeachersWithCoursesAndStudents());

        public IActionResult TopCourses()
            => View(this.statistics
                        .GetTopCourses());

        public IActionResult TopTeachers()
            => View(this.statistics
                            .GetTopTeachers());
    }
}
