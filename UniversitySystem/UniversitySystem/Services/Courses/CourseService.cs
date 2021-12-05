namespace UniversitySystem.Services.Courses
{
    using System;
    using System.Data;
    using System.Collections.Generic;

    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using UniversitySystem.Services.Courses.Models;

    using static WebConstants;

    public class CourseService : ICourseService
    {
        private readonly IConfiguration configuration;

        public CourseService(IConfiguration configuration)
            => this.configuration = configuration;

        public void Create(string name,
            int credit,
            int facultyId,
            ICollection<int> teachers)
        {
            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                var teachersAsString = string.Join(',', teachers);

                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("AddNewCourse", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Name", name);
                cmd.Parameters.AddWithValue("Credit", credit);
                cmd.Parameters.AddWithValue("FacultyId", facultyId);
                cmd.Parameters.AddWithValue("TeacherIds", teachersAsString);

                cmd.ExecuteNonQuery();
            }
        }

        public ICollection<string> GetExistingCourses(int facultyId)
        {
            var courses = new List<string>();

            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                SqlCommand cmd = new SqlCommand($"SELECT Name FROM Courses WHERE FacultyId = {facultyId}");
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    courses.Add(reader["Name"].ToString());
                }
            }

            return courses;
        }

        public ICollection<TeacherServiceModel> GetTeachers(int facultyId)
        {
            var teachers = new List<TeacherServiceModel>();

            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                SqlCommand cmd = new SqlCommand($"SELECT Id, FirstName, LastName FROM Teachers WHERE FacultyId = {facultyId}");
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    teachers.Add(new TeacherServiceModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                    });
                }
            }

            return teachers;
        }
    }
}
