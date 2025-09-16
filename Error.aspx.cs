using System;
using System.Web;

namespace StudentManagementSystem
{
    /// <summary>
    /// Error page code-behind
    /// Handles error display and logging
    /// </summary>
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadErrorMessage();
            }
        }

        /// <summary>
        /// Load and display error message
        /// </summary>
        private void LoadErrorMessage()
        {
            try
            {
                // Get error message from query string
                string errorMessage = Request.QueryString["error"];
                
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    // Decode the error message
                    errorMessage = HttpUtility.UrlDecode(errorMessage);
                    lblErrorMessage.Text = errorMessage;
                }
                else
                {
                    // Default error message
                    lblErrorMessage.Text = "عذراً، حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى أو العودة إلى الصفحة الرئيسية.";
                }
                
                // Log the error for debugging
                LogError(errorMessage);
            }
            catch (Exception ex)
            {
                // If there's an error loading the error message, show a generic message
                lblErrorMessage.Text = "عذراً، حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى أو العودة إلى الصفحة الرئيسية.";
                LogError(ex.Message);
            }
        }

        /// <summary>
        /// Log error for debugging purposes
        /// </summary>
        /// <param name="errorMessage">Error message to log</param>
        private void LogError(string errorMessage)
        {
            try
            {
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR PAGE: {errorMessage}";
                System.Diagnostics.Debug.WriteLine(logMessage);
                
                // In a real application, you would write to a log file or database
                // LogToFile(logMessage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error logging error message: {ex.Message}");
            }
        }
    }
}
