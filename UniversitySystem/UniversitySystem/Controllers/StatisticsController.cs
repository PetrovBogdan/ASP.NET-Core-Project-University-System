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
    }
}
