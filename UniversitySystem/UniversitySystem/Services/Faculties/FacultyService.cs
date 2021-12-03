namespace UniversitySystem.Services.Faculties
{
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using UniversitySystem.Services.Faculties.Models;

    using static WebConstants;
    public class FacultyService : IFacultyService
    {
        private readonly IConfiguration configuration;

        public FacultyService(IConfiguration configuration)
            => this.configuration = configuration;

        public ICollection<FacultyServiceModel> GetAll()
        {
            var faculties = new List<FacultyServiceModel>();

            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM Faculties");
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    faculties.Add(new FacultyServiceModel
                    {
                        Name = reader["Name"].ToString(),
                        Id = Convert.ToInt32(reader["Id"])
                    });
                }
            }

            return faculties;
        }
    }
}
