using System;
using System.Web;
using System.Web.Routing;
using System.Web.SessionState;

namespace StudentManagementSystem
{
    /// <summary>
    /// Global application class
    /// Handles application-level events and configuration
    /// </summary>
    public class Global : HttpApplication
    {
        /// <summary>
        /// Application start event
        /// Initializes application settings and configurations
        /// </summary>
        protected void Application_Start(object sender, EventArgs e)
        {
            // Initialize application settings
            InitializeApplication();
            
            // Register routes if needed
            RegisterRoutes(RouteTable.Routes);
            
            // Log application start
            LogApplicationEvent("Application Started");
        }

        /// <summary>
        /// Session start event
        /// Initializes session-specific settings
        /// </summary>
        protected void Session_Start(object sender, EventArgs e)
        {
            // Initialize session variables
            Session["UserID"] = null;
            Session["UserName"] = null;
            Session["IsAuthenticated"] = false;
            Session["LastActivity"] = DateTime.Now;
            
            // Log session start
            LogSessionEvent("Session Started");
        }

        /// <summary>
        /// Application begin request event
        /// Handles request initialization
        /// </summary>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Set culture for Arabic language support
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("ar-SA");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Application error event
        /// Handles unhandled exceptions
        /// </summary>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            
            if (exception != null)
            {
                // Log the error
                LogError(exception);
                
                // Clear the error to prevent default error page
                Server.ClearError();
                
                // Redirect to custom error page
                Response.Redirect("~/Error.aspx?error=" + HttpUtility.UrlEncode(exception.Message));
            }
        }

        /// <summary>
        /// Session end event
        /// Handles session cleanup
        /// </summary>
        protected void Session_End(object sender, EventArgs e)
        {
            // Log session end
            LogSessionEvent("Session Ended");
            
            // Clean up session resources
            Session.Clear();
        }

        /// <summary>
        /// Application end event
        /// Handles application cleanup
        /// </summary>
        protected void Application_End(object sender, EventArgs e)
        {
            // Log application end
            LogApplicationEvent("Application Ended");
        }

        #region Private Methods

        /// <summary>
        /// Initialize application settings
        /// </summary>
        private void InitializeApplication()
        {
            try
            {
                // Set application variables
                Application["ApplicationName"] = "نظام إدارة الطلاب";
                Application["Version"] = "1.0.0";
                Application["StartTime"] = DateTime.Now;
                Application["IsOnline"] = true;
                
                // Initialize database connection
                InitializeDatabase();
                
                // Set up error handling
                SetupErrorHandling();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        /// <summary>
        /// Initialize database connection
        /// </summary>
        private void InitializeDatabase()
        {
            try
            {
                // Test database connection
                using (var connection = new System.Data.SqlClient.SqlConnection(
                    System.Configuration.ConfigurationManager.ConnectionStrings["StudentDBConnection"].ConnectionString))
                {
                    connection.Open();
                    Application["DatabaseStatus"] = "Connected";
                }
            }
            catch (Exception ex)
            {
                Application["DatabaseStatus"] = "Disconnected";
                LogError(ex);
            }
        }

        /// <summary>
        /// Setup error handling configuration
        /// </summary>
        private void SetupErrorHandling()
        {
            // Enable custom errors
            System.Web.Configuration.CustomErrorsSection customErrors = 
                (System.Web.Configuration.CustomErrorsSection)System.Configuration.ConfigurationManager.GetSection("system.web/customErrors");
            
            if (customErrors != null)
            {
                Application["CustomErrorsEnabled"] = customErrors.Mode != System.Web.Configuration.CustomErrorsMode.Off;
            }
        }

        /// <summary>
        /// Register application routes
        /// </summary>
        /// <param name="routes">Route collection</param>
        private void RegisterRoutes(RouteCollection routes)
        {
            // Register custom routes if needed
            // Example: routes.MapPageRoute("StudentDetails", "student/{id}", "~/StudentDetails.aspx");
        }

        /// <summary>
        /// Log application events
        /// </summary>
        /// <param name="message">Event message</param>
        private void LogApplicationEvent(string message)
        {
            try
            {
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] APPLICATION: {message}";
                System.Diagnostics.Debug.WriteLine(logMessage);
                
                // In a real application, you would write to a log file or database
                // LogToFile(logMessage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error logging application event: {ex.Message}");
            }
        }

        /// <summary>
        /// Log session events
        /// </summary>
        /// <param name="message">Event message</param>
        private void LogSessionEvent(string message)
        {
            try
            {
                string sessionId = Session?.SessionID ?? "Unknown";
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] SESSION [{sessionId}]: {message}";
                System.Diagnostics.Debug.WriteLine(logMessage);
                
                // In a real application, you would write to a log file or database
                // LogToFile(logMessage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error logging session event: {ex.Message}");
            }
        }

        /// <summary>
        /// Log errors
        /// </summary>
        /// <param name="exception">Exception to log</param>
        private void LogError(Exception exception)
        {
            try
            {
                string errorMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {exception.Message}\n" +
                                    $"Stack Trace: {exception.StackTrace}\n" +
                                    $"Source: {exception.Source}";
                
                System.Diagnostics.Debug.WriteLine(errorMessage);
                
                // In a real application, you would write to a log file or database
                // LogToFile(errorMessage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error logging error: {ex.Message}");
            }
        }

        #endregion
    }
}
