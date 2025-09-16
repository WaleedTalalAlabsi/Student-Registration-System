using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace StudentManagementSystem
{
    /// <summary>
    /// StudentManager class handles all database operations for students
    /// This class demonstrates the use of ADO.NET, SQL operations, and error handling
    /// </summary>
    public class StudentManager
    {
        #region Private Fields

        private string connectionString;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor that initializes the database connection string
        /// </summary>
        public StudentManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["StudentDBConnection"].ConnectionString;
        }

        #endregion

        #region CRUD Operations

        /// <summary>
        /// Adds a new student to the database
        /// Demonstrates INSERT operation and parameterized queries
        /// </summary>
        /// <param name="student">Student object to add</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool AddStudent(Student student)
        {
            // Validate student data
            if (!student.IsValid())
            {
                throw new ArgumentException("بيانات الطالب غير صحيحة");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if student ID already exists
                    if (StudentExists(student.StudentID))
                    {
                        return false;
                    }

                    string query = @"
                        INSERT INTO Students (StudentID, StudentName, Email, Phone, Department, GPA, Status, EnrollmentDate, CreatedDate)
                        VALUES (@StudentID, @StudentName, @Email, @Phone, @Department, @GPA, @Status, @EnrollmentDate, @CreatedDate)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@StudentID", student.StudentID);
                        command.Parameters.AddWithValue("@StudentName", student.StudentName);
                        command.Parameters.AddWithValue("@Email", (object)student.Email ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Phone", (object)student.Phone ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Department", student.Department);
                        command.Parameters.AddWithValue("@GPA", (object)student.GPA ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Status", student.Status);
                        command.Parameters.AddWithValue("@EnrollmentDate", (object)student.EnrollmentDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@CreatedDate", student.CreatedDate);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the error (in a real application, you would use a logging framework)
                System.Diagnostics.Debug.WriteLine($"SQL Error in AddStudent: {ex.Message}");
                throw new Exception("خطأ في قاعدة البيانات: " + ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in AddStudent: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates an existing student in the database
        /// Demonstrates UPDATE operation
        /// </summary>
        /// <param name="student">Student object with updated information</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdateStudent(Student student)
        {
            if (!student.IsValid())
            {
                throw new ArgumentException("بيانات الطالب غير صحيحة");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        UPDATE Students 
                        SET StudentName = @StudentName, 
                            Email = @Email, 
                            Phone = @Phone, 
                            Department = @Department, 
                            GPA = @GPA, 
                            Status = @Status, 
                            EnrollmentDate = @EnrollmentDate,
                            UpdatedDate = @UpdatedDate
                        WHERE StudentID = @StudentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentID", student.StudentID);
                        command.Parameters.AddWithValue("@StudentName", student.StudentName);
                        command.Parameters.AddWithValue("@Email", (object)student.Email ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Phone", (object)student.Phone ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Department", student.Department);
                        command.Parameters.AddWithValue("@GPA", (object)student.GPA ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Status", student.Status);
                        command.Parameters.AddWithValue("@EnrollmentDate", (object)student.EnrollmentDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"SQL Error in UpdateStudent: {ex.Message}");
                throw new Exception("خطأ في قاعدة البيانات: " + ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in UpdateStudent: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes a student from the database
        /// Demonstrates DELETE operation
        /// </summary>
        /// <param name="studentID">ID of the student to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool DeleteStudent(string studentID)
        {
            if (string.IsNullOrWhiteSpace(studentID))
            {
                throw new ArgumentException("رقم الطالب مطلوب");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Students WHERE StudentID = @StudentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentID", studentID);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"SQL Error in DeleteStudent: {ex.Message}");
                throw new Exception("خطأ في قاعدة البيانات: " + ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in DeleteStudent: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves a specific student by ID
        /// Demonstrates SELECT operation with WHERE clause
        /// </summary>
        /// <param name="studentID">ID of the student to retrieve</param>
        /// <returns>Student object or null if not found</returns>
        public Student GetStudentByID(string studentID)
        {
            if (string.IsNullOrWhiteSpace(studentID))
            {
                return null;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT StudentID, StudentName, Email, Phone, Department, GPA, Status, 
                               EnrollmentDate, CreatedDate, UpdatedDate
                        FROM Students 
                        WHERE StudentID = @StudentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentID", studentID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapDataReaderToStudent(reader);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"SQL Error in GetStudentByID: {ex.Message}");
                throw new Exception("خطأ في قاعدة البيانات: " + ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in GetStudentByID: {ex.Message}");
                throw;
            }

            return null;
        }

        /// <summary>
        /// Retrieves all students from the database
        /// Demonstrates SELECT operation without WHERE clause
        /// </summary>
        /// <returns>List of all students</returns>
        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT StudentID, StudentName, Email, Phone, Department, GPA, Status, 
                               EnrollmentDate, CreatedDate, UpdatedDate
                        FROM Students 
                        ORDER BY StudentName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                students.Add(MapDataReaderToStudent(reader));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"SQL Error in GetAllStudents: {ex.Message}");
                throw new Exception("خطأ في قاعدة البيانات: " + ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in GetAllStudents: {ex.Message}");
                throw;
            }

            return students;
        }

        #endregion

        #region Search Operations

        /// <summary>
        /// Searches students by name and department
        /// Demonstrates SELECT with LIKE operator and multiple conditions
        /// </summary>
        /// <param name="searchTerm">Search term for name</param>
        /// <param name="department">Department filter</param>
        /// <returns>List of matching students</returns>
        public List<Student> SearchStudents(string searchTerm, string department)
        {
            List<Student> students = new List<Student>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT StudentID, StudentName, Email, Phone, Department, GPA, Status, 
                               EnrollmentDate, CreatedDate, UpdatedDate
                        FROM Students 
                        WHERE (@SearchTerm IS NULL OR @SearchTerm = '' OR StudentName LIKE @SearchPattern)
                        AND (@Department IS NULL OR @Department = '' OR Department = @Department)
                        ORDER BY StudentName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SearchTerm", string.IsNullOrWhiteSpace(searchTerm) ? (object)DBNull.Value : searchTerm);
                        command.Parameters.AddWithValue("@SearchPattern", string.IsNullOrWhiteSpace(searchTerm) ? (object)DBNull.Value : "%" + searchTerm + "%");
                        command.Parameters.AddWithValue("@Department", string.IsNullOrWhiteSpace(department) ? (object)DBNull.Value : department);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                students.Add(MapDataReaderToStudent(reader));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"SQL Error in SearchStudents: {ex.Message}");
                throw new Exception("خطأ في قاعدة البيانات: " + ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in SearchStudents: {ex.Message}");
                throw;
            }

            return students;
        }

        /// <summary>
        /// Advanced search with multiple criteria
        /// Demonstrates complex WHERE clause with multiple conditions
        /// </summary>
        /// <param name="name">Name search term</param>
        /// <param name="studentID">Student ID search term</param>
        /// <param name="department">Department filter</param>
        /// <param name="status">Status filter</param>
        /// <returns>List of matching students</returns>
        public List<Student> AdvancedSearch(string name, string studentID, string department, string status)
        {
            List<Student> students = new List<Student>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT StudentID, StudentName, Email, Phone, Department, GPA, Status, 
                               EnrollmentDate, CreatedDate, UpdatedDate
                        FROM Students 
                        WHERE (@Name IS NULL OR @Name = '' OR StudentName LIKE @NamePattern)
                        AND (@StudentID IS NULL OR @StudentID = '' OR StudentID LIKE @StudentIDPattern)
                        AND (@Department IS NULL OR @Department = '' OR Department = @Department)
                        AND (@Status IS NULL OR @Status = '' OR Status = @Status)
                        ORDER BY StudentName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", string.IsNullOrWhiteSpace(name) ? (object)DBNull.Value : name);
                        command.Parameters.AddWithValue("@NamePattern", string.IsNullOrWhiteSpace(name) ? (object)DBNull.Value : "%" + name + "%");
                        command.Parameters.AddWithValue("@StudentID", string.IsNullOrWhiteSpace(studentID) ? (object)DBNull.Value : studentID);
                        command.Parameters.AddWithValue("@StudentIDPattern", string.IsNullOrWhiteSpace(studentID) ? (object)DBNull.Value : "%" + studentID + "%");
                        command.Parameters.AddWithValue("@Department", string.IsNullOrWhiteSpace(department) ? (object)DBNull.Value : department);
                        command.Parameters.AddWithValue("@Status", string.IsNullOrWhiteSpace(status) ? (object)DBNull.Value : status);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                students.Add(MapDataReaderToStudent(reader));
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"SQL Error in AdvancedSearch: {ex.Message}");
                throw new Exception("خطأ في قاعدة البيانات: " + ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"General Error in AdvancedSearch: {ex.Message}");
                throw;
            }

            return students;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Checks if a student with the given ID already exists
        /// </summary>
        /// <param name="studentID">Student ID to check</param>
        /// <returns>True if exists, false otherwise</returns>
        private bool StudentExists(string studentID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Students WHERE StudentID = @StudentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentID", studentID);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in StudentExists: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Maps SqlDataReader to Student object
        /// Demonstrates object mapping and null handling
        /// </summary>
        /// <param name="reader">SqlDataReader object</param>
        /// <returns>Student object</returns>
        private Student MapDataReaderToStudent(SqlDataReader reader)
        {
            Student student = new Student
            {
                StudentID = reader["StudentID"].ToString(),
                StudentName = reader["StudentName"].ToString(),
                Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString(),
                Phone = reader["Phone"] == DBNull.Value ? null : reader["Phone"].ToString(),
                Department = reader["Department"].ToString(),
                GPA = reader["GPA"] == DBNull.Value ? (double?)null : Convert.ToDouble(reader["GPA"]),
                Status = reader["Status"].ToString(),
                EnrollmentDate = reader["EnrollmentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["EnrollmentDate"]),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                UpdatedDate = reader["UpdatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedDate"])
            };

            return student;
        }

        #endregion

        #region Statistics Methods

        /// <summary>
        /// Gets total number of students
        /// </summary>
        /// <returns>Total count</returns>
        public int GetTotalStudentsCount()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Students";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetTotalStudentsCount: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Gets count of students by status
        /// </summary>
        /// <param name="status">Status to count</param>
        /// <returns>Count of students with the given status</returns>
        public int GetStudentsCountByStatus(string status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Students WHERE Status = @Status";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Status", status);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetStudentsCountByStatus: {ex.Message}");
                return 0;
            }
        }

        #endregion
    }
}
