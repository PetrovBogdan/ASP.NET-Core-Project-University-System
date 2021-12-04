namespace UniversitySystem.Services.Statistics.Models
{
    public abstract class PersonListingServiceModel
    {
        public string FullName { get; set; }

        public abstract object GetStatistics();
    }
}
