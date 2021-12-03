namespace UniversitySystem.Services.Students
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using UniversitySystem.Services.Students.Models;

    using static WebConstants;
    public class StudentService : IStudentService
    {
        private readonly IConfiguration configuration;

        public StudentService(IConfiguration configuration)
            => this.configuration = configuration;

        public void Create(string firstName,
            string lastName,
            int facultyId,
            ICollection<int?> courses)
        {
            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                string coursesAsString = string.Join(',', courses);

                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("AddStudent", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FirstName", firstName);
                cmd.Parameters.AddWithValue("LastName", lastName);
                cmd.Parameters.AddWithValue("FacultyId", facultyId);
                cmd.Parameters.AddWithValue("Courses", coursesAsString);

                cmd.ExecuteNonQuery();
            }
        }

        public ICollection<CourseServiceModel> GetCourses()
        {
            var courses = new List<CourseServiceModel>();

            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM Courses");
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    courses.Add(new CourseServiceModel
                    {
                        Name = reader["Name"].ToString(),
                        Id = Convert.ToInt32(reader["Id"])
                    });
                }
            }

            return courses;
        }
    }
}
