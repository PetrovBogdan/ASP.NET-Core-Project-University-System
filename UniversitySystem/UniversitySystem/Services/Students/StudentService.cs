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

        public StudentDetailsServiceModel GetDetails(int studentId)
        {
            var student = new StudentDetailsServiceModel();

            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                var getStudentsQuery = $"SELECT* FROM GetStudentDetails({studentId})";
                var getActiveCoursesQuery = "SELECT Id, Name FROM Courses";
                SqlCommand cmd = new SqlCommand(getStudentsQuery);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (student.Id == 0)
                    {
                        student.Id = Convert.ToInt32(reader["Student Id"]);
                        student.FirstName = reader["FirstName"].ToString();
                        student.LastName = reader["LastName"].ToString();
                        student.Courses = new List<CourseServiceModel>();

                    }

                    var dbCourseId = reader["Course Id"];

                    if (dbCourseId is System.DBNull)
                    {
                        continue;
                    }

                    student.Courses.Add(new CourseServiceModel
                    {
                        Id = Convert.ToInt32(dbCourseId),
                        Name = reader["Name"].ToString(),
                    });

                }

                cmd = new SqlCommand(getActiveCoursesQuery);
                cmd.Connection = sqlConnection;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    student.ActiveCourses.Add(new CourseServiceModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                    });
                }
            }

            return student;
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

        public void AddCourse(int courseId, int studentId)
        {
            var studentCourse = new Dictionary<int, int>();

            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                string insertQuery = $"INSERT INTO StudentCourses VALUES ({courseId}, {studentId})";
                var getQuery = "SELECT * FROM StudentCourses";
                sqlConnection.Open();

                SqlCommand cmd = new SqlCommand(getQuery, sqlConnection);
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var dbStudentId = Convert.ToInt32(reader["StudentId"]);
                    var dbCourseId = Convert.ToInt32(reader["CourseId"]);

                    if (dbCourseId == courseId && dbStudentId == studentId)
                    {
                        return;
                    }
                }

                cmd = new SqlCommand(insertQuery, sqlConnection);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveCourse(int courseId, int studentId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                string query = $"DELETE FROM StudentCourses WHERE CourseId = {courseId} AND StudentId = {studentId} ";

                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
