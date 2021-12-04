namespace UniversitySystem.Services.Teachers
{
    using System;

    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using UniversitySystem.Services.Teachers.Models;

    using static WebConstants;
    using System.Data;

    public class TeacherService : ITeacherService
    {
        private readonly IConfiguration configuration;

        public TeacherService(IConfiguration configuration)
            => this.configuration = configuration;

        public void Create(string firstName,
            string lastName,
            int facultyId, 
            int titleId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {

                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("AddTeacher", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FirstName", firstName);
                cmd.Parameters.AddWithValue("LastName", lastName);
                cmd.Parameters.AddWithValue("FacultyId", facultyId);
                cmd.Parameters.AddWithValue("TitleId", titleId);

                cmd.ExecuteNonQuery();
            }
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
