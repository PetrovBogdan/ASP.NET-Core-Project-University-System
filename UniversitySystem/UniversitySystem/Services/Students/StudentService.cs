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
            var teacherIds = GetTeachersForCourse(courses);
            string teacherIdsAsString = string.Join(",", teacherIds);

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
                cmd.Parameters.AddWithValue("TeachersIds", teacherIdsAsString);

                cmd.ExecuteNonQuery();
            }
        }

        private ICollection<int> GetTeachersForCourse(ICollection<int?> courses)
        {
            var teacherIds = new HashSet<int>();

            foreach (var courseId in courses)
            {
                using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
                {

                    var query = $"SELECT t.Id FROM Teachers AS t JOIN TeacherCourses AS tc ON t.Id = tc.TeacherId JOIN Courses AS c ON c.Id = tc.CourseId WHERE c.Id = {courseId}";

                    SqlCommand cmd = new SqlCommand(query);
                    cmd.Connection = sqlConnection;
                    sqlConnection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        teacherIds.Add(Convert.ToInt32(reader["Id"]));
                    }

                }
            }
            return teacherIds;
        }

        public ICollection<CourseServiceModel> GetCourses(int facultyId)
        {
            var courses = new List<CourseServiceModel>();

            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                SqlCommand cmd = new SqlCommand($"SELECT Id, Name FROM Courses WHERE FacultyId = {facultyId}");
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
