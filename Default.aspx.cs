using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentManagementSystem
{
    public partial class Default : System.Web.UI.Page
    {
        // Database connection string
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardStats();
                LoadStudents();
            }
        }

        #region Dashboard Methods

        private void LoadDashboardStats()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Get total students count
                    string totalQuery = "SELECT COUNT(*) FROM Students";
                    using (SqlCommand command = new SqlCommand(totalQuery, connection))
                    {
                        int totalStudents = Convert.ToInt32(command.ExecuteScalar());
                        ScriptManager.RegisterStartupScript(this, GetType(), "UpdateTotalStudents", 
                            $"document.getElementById('totalStudents').textContent = '{totalStudents}';", true);
                    }

                    // Get active students count
                    string activeQuery = "SELECT COUNT(*) FROM Students WHERE Status = 'نشط'";
                    using (SqlCommand command = new SqlCommand(activeQuery, connection))
                    {
                        int activeStudents = Convert.ToInt32(command.ExecuteScalar());
                        ScriptManager.RegisterStartupScript(this, GetType(), "UpdateActiveStudents", 
                            $"document.getElementById('activeStudents').textContent = '{activeStudents}';", true);
                    }

                    // Get graduated students count
                    string graduatedQuery = "SELECT COUNT(*) FROM Students WHERE Status = 'خريج'";
                    using (SqlCommand command = new SqlCommand(graduatedQuery, connection))
                    {
                        int graduatedStudents = Convert.ToInt32(command.ExecuteScalar());
                        ScriptManager.RegisterStartupScript(this, GetType(), "UpdateGraduatedStudents", 
                            $"document.getElementById('graduatedStudents').textContent = '{graduatedStudents}';", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("خطأ في تحميل الإحصائيات: " + ex.Message, "error");
            }
        }

        #endregion

        #region Student Management Methods

        protected void btnAddStudent_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Student student = new Student
                    {
                        StudentID = txtStudentID.Text.Trim(),
                        StudentName = txtStudentName.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Phone = txtPhone.Text.Trim(),
                        Department = ddlDepartment.SelectedValue,
                        GPA = string.IsNullOrEmpty(txtGPA.Text) ? (double?)null : Convert.ToDouble(txtGPA.Text),
                        Status = ddlStatus.SelectedValue,
                        EnrollmentDate = string.IsNullOrEmpty(txtEnrollmentDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtEnrollmentDate.Text)
                    };

                    StudentManager studentManager = new StudentManager();
                    bool success = studentManager.AddStudent(student);

                    if (success)
                    {
                        ShowMessage("تم إضافة الطالب بنجاح!", "success");
                        ClearForm();
                        LoadDashboardStats();
                        LoadStudents();
                    }
                    else
                    {
                        ShowMessage("فشل في إضافة الطالب. قد يكون رقم الطالب موجود مسبقاً.", "error");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("خطأ في إضافة الطالب: " + ex.Message, "error");
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtStudentName.Text = "";
            txtStudentID.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            ddlDepartment.SelectedIndex = 0;
            txtGPA.Text = "";
            ddlStatus.SelectedIndex = 0;
            txtEnrollmentDate.Text = "";
        }

        #endregion

        #region View Students Methods

        private void LoadStudents()
        {
            try
            {
                StudentManager studentManager = new StudentManager();
                List<Student> students = studentManager.GetAllStudents();

                gvStudents.DataSource = students;
                gvStudents.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("خطأ في تحميل قائمة الطلاب: " + ex.Message, "error");
            }
        }

        protected void gvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string studentID = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditStudent":
                        EditStudent(studentID);
                        break;
                    case "DeleteStudent":
                        DeleteStudent(studentID);
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("خطأ في تنفيذ العملية: " + ex.Message, "error");
            }
        }

        protected void gvStudents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Add status-based styling
                string status = DataBinder.Eval(e.Row.DataItem, "Status").ToString();
                switch (status)
                {
                    case "نشط":
                        e.Row.CssClass += " status-active";
                        break;
                    case "خريج":
                        e.Row.CssClass += " status-graduated";
                        break;
                    case "منقطع":
                        e.Row.CssClass += " status-inactive";
                        break;
                }
            }
        }

        protected void gvStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvStudents.PageIndex = e.NewPageIndex;
            LoadStudents();
        }

        private void EditStudent(string studentID)
        {
            try
            {
                StudentManager studentManager = new StudentManager();
                Student student = studentManager.GetStudentByID(studentID);

                if (student != null)
                {
                    // Switch to add student tab and populate form
                    ScriptManager.RegisterStartupScript(this, GetType(), "SwitchToAddTab", "showTab('add-student');", true);
                    
                    txtStudentName.Text = student.StudentName;
                    txtStudentID.Text = student.StudentID;
                    txtEmail.Text = student.Email;
                    txtPhone.Text = student.Phone;
                    ddlDepartment.SelectedValue = student.Department;
                    txtGPA.Text = student.GPA?.ToString();
                    ddlStatus.SelectedValue = student.Status;
                    txtEnrollmentDate.Text = student.EnrollmentDate?.ToString("yyyy-MM-dd");

                    // Store original student ID for update
                    ViewState["EditingStudentID"] = studentID;
                    btnAddStudent.Text = "تحديث الطالب";
                }
            }
            catch (Exception ex)
            {
                ShowMessage("خطأ في تحميل بيانات الطالب: " + ex.Message, "error");
            }
        }

        private void DeleteStudent(string studentID)
        {
            try
            {
                StudentManager studentManager = new StudentManager();
                bool success = studentManager.DeleteStudent(studentID);

                if (success)
                {
                    ShowMessage("تم حذف الطالب بنجاح!", "success");
                    LoadDashboardStats();
                    LoadStudents();
                }
                else
                {
                    ShowMessage("فشل في حذف الطالب.", "error");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("خطأ في حذف الطالب: " + ex.Message, "error");
            }
        }

        #endregion

        #region Search Methods

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterStudents();
        }

        protected void ddlFilterDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterStudents();
        }

        private void FilterStudents()
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                string department = ddlFilterDepartment.SelectedValue;

                StudentManager studentManager = new StudentManager();
                List<Student> students = studentManager.SearchStudents(searchTerm, department);

                gvStudents.DataSource = students;
                gvStudents.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("خطأ في البحث: " + ex.Message, "error");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtSearchName.Text.Trim();
                string studentID = txtSearchID.Text.Trim();
                string department = ddlSearchDepartment.SelectedValue;
                string status = ddlSearchStatus.SelectedValue;

                StudentManager studentManager = new StudentManager();
                List<Student> students = studentManager.AdvancedSearch(name, studentID, department, status);

                gvSearchResults.DataSource = students;
                gvSearchResults.DataBind();
                gvSearchResults.Visible = students.Count > 0;

                if (students.Count == 0)
                {
                    ShowMessage("لم يتم العثور على نتائج مطابقة للبحث.", "info");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("خطأ في البحث المتقدم: " + ex.Message, "error");
            }
        }

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearchName.Text = "";
            txtSearchID.Text = "";
            ddlSearchDepartment.SelectedIndex = 0;
            ddlSearchStatus.SelectedIndex = 0;
            gvSearchResults.Visible = false;
        }

        #endregion

        #region Utility Methods

        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"message message-{type}";
            lblMessage.Visible = true;

            // Auto-hide success messages after 3 seconds
            if (type == "success")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "HideMessage", 
                    "setTimeout(function() { document.getElementById('" + lblMessage.ClientID + "').style.display = 'none'; }, 3000);", true);
            }
        }

        #endregion
    }
}
