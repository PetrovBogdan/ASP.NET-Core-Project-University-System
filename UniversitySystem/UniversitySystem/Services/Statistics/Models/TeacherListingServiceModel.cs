namespace UniversitySystem.Services.Statistics.Models
{
    public class TeacherListingServiceModel: PersonListingServiceModel
    {
        public int CoursesCount { get; set; }

        public override object GetStatistics()
        {
            return this.CoursesCount;
        }
    }
}
