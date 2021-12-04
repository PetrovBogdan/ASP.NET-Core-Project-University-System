namespace UniversitySystem.Services.Statistics.Models
{
    public class StudentListingServiceModel : PersonListingServiceModel
    {
        public string Course { get; set; }

        public override string GetStatistics()
        {
            return this.Course;
        }
    }
}
