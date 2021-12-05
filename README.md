# ASP.NET-Core-Project-University-System

The project was created without using the Entity Framework Core, so to run the application successfully, the following steps must be performed:

1. Create the database in SSMS with the following code: 

1.1
CREATE DATABASE UniversitySystem
1.2
USE UniversitySystem
1.3
CREATE TABLE Faculties (
Id INT PRIMARY KEY IDENTITY NOT NULL,
Name NVARCHAR(50) NOT NULL
);

CREATE TABLE Students(
Id INT PRIMARY KEY IDENTITY NOT NULL,
FirstName NVARCHAR(30) NOT NULL,
LastNAme NVARCHAR(30) NOT NULL,
FacultyId INT FOREIGN KEY REFERENCES Faculties (Id) NOT NULL
);

CREATE TABLE Titles(
Id INT PRIMARY KEY IDENTITY NOT NULL,
Type NVARCHAR(30) NOT NULL,
);

CREATE TABLE Teachers(
Id INT PRIMARY KEY IDENTITY,
FirstName NVARCHAR(30) NOT NULL,
LastName NVARCHAR (30) NOT NULL,
FacultyId INT FOREIGN KEY REFERENCES Faculties(id) NOT NULL,
TitleId INT FOREIGN KEY REFERENCES Titles (Id) NOT NULL,
);

CREATE TABLE Courses(
Id INT PRIMARY KEY IDENTITY,
Name NVARCHAR(50) NOT NULL,
Credit INT NOT NULL,
FacultyId INT FOREIGN KEY REFERENCES Faculties(Id) NOT NULL
);

CREATE TABLE StudentCourses(
CourseId INT FOREIGN KEY REFERENCES Courses(Id) NOT NULL,
StudentId INT FOREIGN KEY REFERENCES Students(Id) NOT NULL,
PRIMARY KEY (CourseId, StudentId)
);

CREATE TABLE TeacherStudents(
StudentId INT FOREIGN KEY REFERENCES Students(Id) NOT NULL,
TeacherId INT FOREIGN KEY REFERENCES Teachers(Id) NOT NULL,
PRIMARY KEY (StudentId, TeacherId),
);

CREATE TABLE TeacherCourses(
TeacherId INT FOREIGN KEY REFERENCES Teachers(Id) NOT NULL,
CourseId INT FOREIGN KEY REFERENCES Courses(Id) NOT NULL,
PRIMARY KEY (TeacherId, CourseId)
);

2. You must insert faculty information and titles for the tachers: 

INSERT INTO Faculties
VALUES
('Computer Science')

INSERT INTO Titles
VALUES
('Assistant Professor'),
('Senior Assistant Professor'),
('Associate Professor'),
('Professor')

3. The following views must be created: 

CREATE VIEW StudentsWihActiveCourses AS 
SELECT s.Id, s.FirstName + ' ' + s.LastNAme AS [Full Name], c.Name
FROM dbo.Students AS s 
LEFT OUTER JOIN dbo.StudentCourses AS sc ON s.Id = sc.StudentId 
LEFT OUTER JOIN dbo.Courses AS c ON sc.CourseId = c.Id


CREATE VIEW StudentsWithCredits AS 
SELECT s.FirstName + ' ' + s.LastNAme AS [Full Name],
SUM(c.Credit) AS [Total Credits]
FROM dbo.Students AS s 
INNER JOIN dbo.StudentCourses AS sc ON s.Id = sc.StudentId 
INNER JOIN dbo.Courses AS c ON c.Id = sc.CourseId
GROUP BY s.FirstName, s.LastNAme


CREATE VIEW TeachersWithCoursesAndStudentsCount AS
SELECT FirstName + ' ' + LastName AS [Teacher Full Name],
Name AS Course, 
[Students Count]
FROM     
      (SELECT t.FirstName, t.LastName, c.Name, COUNT(s.Id) AS [Students Count]
       FROM dbo.Teachers AS t 
       LEFT OUTER JOIN dbo.TeacherCourses AS tc ON t.Id = tc.TeacherId 
       LEFT OUTER JOIN dbo.Courses AS c ON c.Id = tc.CourseId 
       LEFT OUTER JOIN dbo.StudentCourses AS sc ON c.Id = sc.CourseId 
       LEFT OUTER JOIN dbo.Students AS s ON s.Id = sc.StudentId
       GROUP BY t.FirstName, t.LastName, c.Name) AS sub



CREATE VIEW TeachersWithCoursesCount AS
SELECT t.FirstName + ' ' + t.LastName AS [Full Name],
COUNT(c.Id) AS [Courses Count]
FROM dbo.Teachers AS t 
LEFT OUTER JOIN dbo.TeacherCourses AS tc ON t.Id = tc.TeacherId
LEFT OUTER JOIN dbo.Courses AS c ON tc.CourseId = c.Id
GROUP BY t.FirstName, t.LastName


CREATE VIEW TopThreeCourses AS
SELECT TOP (3) c.Name AS Course,
COUNT(s.Id) AS [Total students]
FROM dbo.Courses AS c 
INNER JOIN dbo.StudentCourses AS sc ON c.Id = sc.CourseId
INNER JOIN dbo.Students AS s ON s.Id = sc.StudentId
GROUP BY c.Name
ORDER BY [Total students] DESC


CREATE VIEW TopThreeTeachers AS 
SELECT TOP (3) FirstName + ' ' + LastName AS [Full Name],
SUM(StudentCount) AS [Total Students]
FROM 
      (SELECT t.FirstName, t.LastName, c.Name,
       COUNT(s.Id) AS StudentCount
       FROM dbo.Teachers AS t
       FULL OUTER JOIN dbo.TeacherCourses AS tc ON t.Id = tc.TeacherId 
       FULL OUTER JOIN dbo.Courses AS c ON c.Id = tc.CourseId
       FULL OUTER JOIN dbo.StudentCourses AS sc ON c.Id = sc.CourseId
       FULL OUTER JOIN dbo.Students AS s ON s.Id = sc.StudentId
                  GROUP BY t.FirstName, t.LastName, c.Name) AS sub
GROUP BY FirstName, LastName
ORDER BY [Total Students] DESC

4. The following stored procedures must be created:

CREATE PROCEDURE [dbo].[AddNewCourse]
@Name NVARCHAR(30),
@Credit INT,
@FacultyId INT, 
@TeacherIds NVARCHAR(MAX)
AS
BEGIN 
INSERT INTO Courses
VALUES
(@Name, @Credit, @FacultyId)

DECLARE @CourseId INT;

SET @CourseId = (SELECT TOP (1) Id FROM Courses
					ORDER BY Id DESC)

DECLARE @i INT
SET @i = 1;

DECLARE @CurrTeacherId INT;

WHILE 1 = 1 
BEGIN
SET @CurrTeacherId = (SELECT value FROM 
								(SELECT value, ROW_NUMBER() OVER (ORDER BY value ASC) AS row_num
								FROM STRING_SPLIT(@TeacherIds,',')) AS sub
								WHERE row_num = @i)
IF @CurrTeacherId IS NULL
BEGIN
BREAK
END 

INSERT INTO TeacherCourses
VALUES
(@CurrTeacherId  ,@CourseId)

SET @i += 1;

END 
END


CREATE PROCEDURE [dbo].[AddStudent]
@FirstName NVARCHAR(30),
@LastName NVARCHAR(30),
@FacultyId INT,
@Courses NVARCHAR(MAX),
@TeachersIds NVARCHAR(MAX)
AS
BEGIN
INSERT INTO Students
VALUES
(@FirstName,@LastName,@FacultyId)

DECLARE @StudentID INT;
SET @StudentId = (SELECT TOP(1) Id FROM Students
ORDER BY Id DESC);

DECLARE @i INT
SET @i = 1;

DECLARE @CurrCourseId NVARCHAR(MAX);

WHILE 1 = 1 
BEGIN
SET @CurrCourseId = (SELECT value FROM 
								(SELECT value, ROW_NUMBER() OVER (ORDER BY value ASC) AS row_num
								FROM STRING_SPLIT(@Courses,',')) AS sub
								WHERE row_num = @i)

IF @CurrCourseId IS NULL
BEGIN 
BREAK 
END 

INSERT INTO StudentCourses
VALUES
(CAST(@CurrCourseId AS INT) ,@StudentID)

SET @i += 1;

END 

DECLARE @j INT
SET @j = 1;

DECLARE @CurrTeacherId NVARCHAR(MAX);

WHILE 1 = 1
BEGIN
SET @CurrTeacherId = (SELECT value FROM 
								(SELECT value, ROW_NUMBER() OVER (ORDER BY value ASC ) AS row_num
								FROM STRING_SPLIT(@TeachersIds,',')) AS sub
								WHERE row_num = @j)

IF @CurrTeacherId IS NULL
BEGIN
BREAK
END

INSERT INTO TeacherStudents
VALUES
(@StudentID,CAST(@CurrTeacherId AS INT))

SET @j += 1;
END

END


CREATE PROCEDURE [dbo].[AddTeacher]
@FirstName NVARCHAR(30),
@LastName NVARCHAR(30),
@FacultyId INT,
@TitleId INT
AS
BEGIN
INSERT INTO Teachers
VALUES
(@FirstName,@LastName,@FacultyId,@TitleId)
END

5.The following function must be created: 

CREATE FUNCTION [dbo].[GetStudentDetails] (@Id INT)
RETURNS TABLE
AS
RETURN 
SELECT s.Id AS [Student Id],
s.FirstName, s.LastNAme, 
c.Id AS [Course Id],
c.Name
FROM     dbo.Students AS s 
LEFT JOIN dbo.StudentCourses AS sc ON s.Id = sc.StudentId 
LEFT JOIN dbo.Courses AS c ON sc.CourseId = c.Id
WHERE s.Id = @Id


After inserting all of the above information into the database you can start using the application.
First, you must insert teachers from the hire teachers button.  
After this, courses must be created with the create course button.  
After this you can add students to the created courses with teachers with the add students’ button. 
After inserting as much information as you need you can check the statistics view by clicking the statistics button.
In order to add or remove courses from students you must click the show students with active courses button in the statistics view.
A table will be loaded in which every name of students is a button that redirects you to a view in which you can see the active courses for the student.
From there you can quit a course or start a new one.


