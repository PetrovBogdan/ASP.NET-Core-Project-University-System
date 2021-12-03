namespace UniversitySystem.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using UniversitySystem.Services.Faculties;

    public class FacultiesController : Controller
    {
        private readonly IFacultyService faculties;

        public FacultiesController(IFacultyService faculties)
            => this.faculties = faculties;

        public IActionResult All(string type)
            => View(this.faculties.GetAll(type));
    }
}
