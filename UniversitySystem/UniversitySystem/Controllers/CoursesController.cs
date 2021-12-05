namespace UniversitySystem.Controllers
{
    using System.Linq;

    using UniversitySystem.Models;
    using Microsoft.AspNetCore.Mvc;
    using UniversitySystem.Services.Courses;

    public class CoursesController : Controller
    {
        private readonly ICourseService course;

        public CoursesController(ICourseService course)
            => this.course = course;

        public IActionResult Create(int facultyId)
        {
            return View(new CourseFormModel
            {
                Courses = this.course.GetExistingCourses(facultyId),
                Teachers = this.course.GetTeachers(facultyId)
            });
        }

        [HttpPost]
        public IActionResult Create(CourseFormModel course,
            int facultyId)
        {
            this.ValidateState(course, facultyId);

            if (!this.ModelState.IsValid)
            {
                return View(new CourseFormModel
                {
                    Courses = this.course.GetExistingCourses(facultyId),
                    Teachers = this.course.GetTeachers(facultyId)
                });
            }

            this.course.Create(course.Name,
                course.Credit,
                facultyId,
                course.TeacherId);

            return RedirectToAction("Index", "Home");
        }

        private void ValidateState(CourseFormModel course,
            int facultyId)
        {
            if (course.TeacherId == null)
            {
                this.ModelState.AddModelError(nameof(course.Teachers),
                    "You must choose at least one teacher for the new course !");
            }

            if (course.Credit < 3 || course.Credit > 35)
            {
                this.ModelState.AddModelError(nameof(course.Credit),
                    "The minimum credit for course is 3 and the maximum is 35");
            }

            course.Courses = this.course.GetExistingCourses(facultyId);

            if (course.Courses.Any(c => c == course.Name))
            {
                this.ModelState.AddModelError(nameof(course.Name),
                    "The course that you are trying to create already existis.");
            }
        }
    }
}
