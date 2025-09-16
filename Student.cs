using System;

namespace StudentManagementSystem
{
    /// <summary>
    /// Class representing a Student entity
    /// This class demonstrates the use of properties, constructors, and data encapsulation
    /// </summary>
    public class Student
    {
        #region Properties

        /// <summary>
        /// Student ID - Primary key
        /// </summary>
        public string StudentID { get; set; }

        /// <summary>
        /// Full name of the student
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// Email address of the student
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Phone number of the student
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Department the student belongs to
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Grade Point Average (GPA) - nullable double
        /// </summary>
        public double? GPA { get; set; }

        /// <summary>
        /// Current status of the student (Active, Graduated, Inactive)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Date when the student enrolled
        /// </summary>
        public DateTime? EnrollmentDate { get; set; }

        /// <summary>
        /// Date when the record was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the record was last updated
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Student()
        {
            CreatedDate = DateTime.Now;
        }

        /// <summary>
        /// Parameterized constructor for creating a new student
        /// </summary>
        /// <param name="studentID">Student ID</param>
        /// <param name="studentName">Student name</param>
        /// <param name="department">Department</param>
        /// <param name="status">Status</param>
        public Student(string studentID, string studentName, string department, string status)
        {
            StudentID = studentID;
            StudentName = studentName;
            Department = department;
            Status = status;
            CreatedDate = DateTime.Now;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the student data
        /// </summary>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsValid()
        {
            // Check required fields
            if (string.IsNullOrWhiteSpace(StudentID) || 
                string.IsNullOrWhiteSpace(StudentName) || 
                string.IsNullOrWhiteSpace(Department) || 
                string.IsNullOrWhiteSpace(Status))
            {
                return false;
            }

            // Validate email format if provided
            if (!string.IsNullOrWhiteSpace(Email) && !IsValidEmail(Email))
            {
                return false;
            }

            // Validate GPA range if provided
            if (GPA.HasValue && (GPA < 0 || GPA > 4))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates email format using simple regex
        /// </summary>
        /// <param name="email">Email to validate</param>
        /// <returns>True if valid email format</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the student's academic level based on GPA
        /// </summary>
        /// <returns>Academic level description</returns>
        public string GetAcademicLevel()
        {
            if (!GPA.HasValue)
                return "غير محدد";

            double gpa = GPA.Value;

            if (gpa >= 3.5)
                return "ممتاز";
            else if (gpa >= 3.0)
                return "جيد جداً";
            else if (gpa >= 2.5)
                return "جيد";
            else if (gpa >= 2.0)
                return "مقبول";
            else
                return "ضعيف";
        }

        /// <summary>
        /// Gets a formatted string representation of the student
        /// </summary>
        /// <returns>Formatted student information</returns>
        public override string ToString()
        {
            return $"الطالب: {StudentName} - الرقم: {StudentID} - القسم: {Department} - الحالة: {Status}";
        }

        /// <summary>
        /// Updates the student's information
        /// </summary>
        /// <param name="studentName">New student name</param>
        /// <param name="email">New email</param>
        /// <param name="phone">New phone</param>
        /// <param name="department">New department</param>
        /// <param name="gpa">New GPA</param>
        /// <param name="status">New status</param>
        public void UpdateInfo(string studentName, string email, string phone, 
            string department, double? gpa, string status)
        {
            StudentName = studentName;
            Email = email;
            Phone = phone;
            Department = department;
            GPA = gpa;
            Status = status;
            UpdatedDate = DateTime.Now;
        }

        #endregion
    }
}
