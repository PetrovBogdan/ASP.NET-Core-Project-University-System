namespace UniversitySystem.Services.Statistics
{
    using System.Collections.Generic;
    using UniversitySystem.Services.Statistics.Models;

    public interface IStatisticsService
    {
        public IDictionary<string, List<PersonListingServiceModel>> GetPeople();
    }
}
