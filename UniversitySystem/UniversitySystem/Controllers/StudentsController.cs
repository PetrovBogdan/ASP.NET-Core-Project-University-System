namespace UniversitySystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using UniversitySystem.Models;
    using UniversitySystem.Services.Students;

    public class StudentsController : Controller
    {
        private readonly IStudentService student;

        public StudentsController(IStudentService student)
            => this.student = student;

        public IActionResult Create(int facultyId)
        {
            return View(new StudentFormModel
            {
                Courses = this.student.GetCourses(facultyId)
            });
        }

        [HttpPost]
        public IActionResult Create(StudentFormModel student,
            int facultyId)
        {
            if (student.CourseId == null)
            {
                this.ModelState.AddModelError(nameof(student.Courses),
                    "You must enter at least 1 course which the student is joining !");
            }

            if (!ModelState.IsValid)
            {
                return View(new StudentFormModel
                {
                    Courses = this.student.GetCourses(facultyId)
                });
            }

            this.student.Create(student.FirstName,
                student.LastName,
                facultyId,
                student.CourseId);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Details(int id)
            => View(this.student
                            .GetDetails(id));

        [HttpPost]
        public IActionResult AddCourse(string courseStudentIds)
        {
            var splittedIds = courseStudentIds.Split(",");
            var courseId = int.Parse(splittedIds[0]);
            var studentId = int.Parse(splittedIds[1]);

            this.student.AddCourse(courseId, studentId);

            return RedirectToAction(nameof(Details), new { id = studentId });
        }

        [HttpPost]
        public IActionResult RemoveCourse(string courseStudentIds)
        {
            var splittedIds = courseStudentIds.Split(",");
            var courseId = int.Parse(splittedIds[0]);
            var studentId = int.Parse(splittedIds[1]);

            this.student.RemoveCourse(courseId, studentId);

            return RedirectToAction(nameof(Details), new { id = studentId });
        }
    }
}
