namespace UniversitySystem.Services.Statistics
{
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using UniversitySystem.Services.Statistics.Models;

    using static WebConstants;
    public class StatisticsService : IStatisticsService
    {
        private readonly IConfiguration configuration;

        public StatisticsService(IConfiguration configuration)
            => this.configuration = configuration;

        public IDictionary<string, List<PersonListingServiceModel>> GetPeople()
        {
            var people = new Dictionary<string, List<PersonListingServiceModel>>();

            GetTeachers(people);
            GetStudents(people);

            return people;

        }

        private void GetStudents(Dictionary<string, List<PersonListingServiceModel>> people)
        {
            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {

                var query = $"SELECT * FROM StudentsWihActiveCourses ORDER BY [Full Name]";

                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (!people.ContainsKey("students"))
                    {
                        people["students"] = new List<PersonListingServiceModel>();
                    }

                    people["students"].Add(new StudentListingServiceModel
                    {
                        FullName = reader["Full Name"].ToString(),
                        Course = reader["Name"].ToString()
                    });
                }

            }
        }

        private void GetTeachers(Dictionary<string, List<PersonListingServiceModel>> people)
        {
            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {

                var query = $"SELECT * FROM TeachersWithCoursesCount ORDER BY [Full Name]";

                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (!people.ContainsKey("teachers"))
                    {
                        people["teachers"] = new List<PersonListingServiceModel>();
                    }

                    people["teachers"].Add(new TeacherListingServiceModel
                    {
                        FullName = reader["Full Name"].ToString(),
                        CoursesCount = Convert.ToInt32(reader["Courses Count"])
                    });
                }

            }
        }
    }
}
