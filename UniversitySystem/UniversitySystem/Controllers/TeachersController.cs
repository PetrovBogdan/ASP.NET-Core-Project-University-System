namespace UniversitySystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using UniversitySystem.Models;
    using UniversitySystem.Services.Teachers;

    public class TeachersController : Controller
    {
        private readonly ITeacherService teacher;

        public TeachersController(ITeacherService teacher)
            => this.teacher = teacher;

        public IActionResult Create()
        {
            return View(new TeacherFormModel
            {
                Titles = this.teacher.GetTitles()
            });
        }

        [HttpPost]
        public IActionResult Create(TeacherFormModel teacher,
            int facultyId)
        {
            if (teacher.TitleId == 0)
            {
                this.ModelState.AddModelError(nameof(teacher.Titles),
                    "You must select title !");
            }

            if (!this.ModelState.IsValid)
            {
                return View(new TeacherFormModel
                {
                    Titles = this.teacher.GetTitles()
                });
            }

            this.teacher.Create(teacher.FirstName,
                teacher.LastName,
                facultyId,
                teacher.TitleId);

            return RedirectToAction("Index", "Home");

        }
    }
}
