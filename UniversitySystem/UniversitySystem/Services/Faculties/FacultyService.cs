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

        public ICollection<FacultyServiceModel> GetAll(string typeCreating)
        {
            var faculties = new List<FacultyServiceModel>();
            var query = "SELECT Id, Name FROM Faculties";

            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    faculties.Add(new FacultyServiceModel
                    {
                        Name = reader["Name"].ToString(),
                        Id = Convert.ToInt32(reader["Id"]),
                        TypeCreating = typeCreating,
                    });
                }
            }

            return faculties;
        }
    }
}
