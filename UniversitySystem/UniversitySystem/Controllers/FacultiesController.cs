namespace UniversitySystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using UniversitySystem.Services.Faculties;
    using UniversitySystem.Services.Faculties.Models;

    public class FacultiesController : Controller
    {
        private readonly IFacultyService faculties;

        public FacultiesController(IFacultyService faculties)
            => this.faculties = faculties;

        public IActionResult All()
            => View(this.faculties.GetAll());
    }
}
