using System;
using System.Data.SqlClient;
using System.Configuration;

namespace AMS
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null && Session["Role"] != null && Session["Role"].ToString() == "Admin")
                {
                    CalculateTotals();
                }
                else
                {
                    // Redirect to login page if session data is missing or user is not an admin
                    Response.Redirect("index.aspx");
                }
            }
        }

        private void CalculateTotals()
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                // Calculate total students
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Students", connection))
                {
                    lblTotalStudents.Text = cmd.ExecuteScalar().ToString();
                }

                // Calculate total teachers
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Teachers", connection))
                {
                    lblTotalTeachers.Text = cmd.ExecuteScalar().ToString();
                }

                // Calculate total departments
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Departments", connection))
                {
                    lblTotalDepartments.Text = cmd.ExecuteScalar().ToString();
                }

                // Calculate total subjects
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Subjects", connection))
                {
                    lblTotalSubjects.Text = cmd.ExecuteScalar().ToString();
                }
            }
        }
    }
}
