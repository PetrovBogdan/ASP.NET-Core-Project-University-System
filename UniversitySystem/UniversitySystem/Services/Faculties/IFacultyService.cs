namespace UniversitySystem.Services.Faculties
{
    using System.Collections.Generic;
    using UniversitySystem.Services.Faculties.Models;

    public interface IFacultyService
    {
        public ICollection<FacultyServiceModel> GetAll();

    }
}
