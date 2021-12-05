namespace UniversitySystem.Services.Statistics
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
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

        public ICollection<StudentCourseServiceModel> GetStudentsWithCourses()
        {
            var students = new List<StudentCourseServiceModel>();

            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                var query = $"SELECT * FROM StudentsWihActiveCourses";

                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var currId = Convert.ToInt32(reader["Id"]);
                    var currCourse = reader["Name"].ToString();

                    if (students != null)
                    {
                        if (students.Any(x => x.Id == currId))
                        {
                            students.FirstOrDefault(x => x.Id == currId).Courses.Add(currCourse);
                            continue;
                        }
                    }

                    students.Add(new StudentCourseServiceModel
                    {
                        Id = currId,
                        FullName = reader["Full Name"].ToString(),
                    });

                    students.FirstOrDefault(x => x.Id == currId).Courses.Add(currCourse);
                }
            }

            return students;
        }

        public ICollection<StudentCreditsServiceModel> GetStudentsWithCredits()
        {
            var students = new List<StudentCreditsServiceModel>();

            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                var query = $"SELECT * FROM StudentsWithCredits";

                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    students.Add(new StudentCreditsServiceModel
                    {
                        FullName = reader["Full Name"].ToString(),
                        Credits = Convert.ToInt32(reader["Total Credits"]),
                    });
                }

            }

            return students;
        }

        public ICollection<TeacherCourseStudentsServiceModel> GetTeachersWithCoursesAndStudents()
        {
            var teachers = new List<TeacherCourseStudentsServiceModel>();

            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                var query = $"SELECT * FROM TeachersWithCoursesAndStudentsCount";

                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    teachers.Add(new TeacherCourseStudentsServiceModel
                    {
                        TeacherFullName = reader["Teacher Full Name"].ToString(),
                        Course = reader["Course"].ToString(),
                        StudentsCount = Convert.ToInt32(reader["Students Count"]),
                    });
                }

            }

            return teachers;
        }

        public ICollection<CourseStatisticsServiceModel> GetTopCourses()
        {
            var courses = new List<CourseStatisticsServiceModel>();

            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                var query = $"SELECT * FROM TopThreeCourses";

                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    courses.Add(new CourseStatisticsServiceModel
                    {
                        Name = reader["Course"].ToString(),
                        TotalStudents = Convert.ToInt32(reader["Total Students"]),
                    });
                }

            }

            return courses;
        }

        public ICollection<TeacherServiceModel> GetTopTeachers()
        {
            var teachers = new List<TeacherServiceModel>();

            using (SqlConnection sqlConnection = new SqlConnection(this.configuration.GetConnectionString(ConnectionString)))
            {
                var query = $"SELECT * FROM TopThreeTeachers";

                SqlCommand cmd = new SqlCommand(query);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    teachers.Add(new TeacherServiceModel
                    {
                        FullName = reader["Full Name"].ToString(),
                        TotalStudents = Convert.ToInt32(reader["Total Students"]),
                    });
                }

            }

            return teachers;
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
