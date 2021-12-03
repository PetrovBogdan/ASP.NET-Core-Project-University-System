namespace UniversitySystem.Services.Teachers
{
    using System;

    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using UniversitySystem.Services.Teachers.Models;

    using static WebConstants;
    public class TeacherService : ITeacherService
    {
        private readonly IConfiguration configuration;

        public TeacherService(IConfiguration configuration)
            => this.configuration = configuration;

        public void Create()
        {

        }

        public ICollection<TitleServiceModel> GetTitles()
        {
            var titles = new List<TitleServiceModel>();

            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                SqlCommand cmd = new SqlCommand($"SELECT Id, Type FROM Titles");
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    titles.Add(new TitleServiceModel
                    {
                        Name = reader["Type"].ToString(),
                        Id = Convert.ToInt32(reader["Id"])
                    });
                }
            }

            return titles;
        }
    }
}
