namespace UniversitySystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using UniversitySystem.Models;
    using UniversitySystem.Services.Students;

    public class StudentsController : Controller
    {
        private readonly IStudentService student;

        public StudentsController(IStudentService student)
            => this.student = student;

        public IActionResult Create()
        {
            return View(new StudentFormModel
            {
                Courses = this.student.GetCourses()
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
                    Courses = this.student.GetCourses()
                });
            }

            this.student.Create(student.FirstName,
                student.LastName,
                facultyId,
                student.CourseId);

            return RedirectToAction("Index", "Home");
        }
    }
}
