using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS
{
    public partial class AttendanceReports : System.Web.UI.Page
    {
        protected GridView gvAttendanceReport;
        protected DropDownList ddlSemester;
        protected TextBox txtDate;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSemesters();
                LoadAttendanceData();
            }
        }

        private void LoadSemesters()
        {
            // Load semester options dynamically
            ddlSemester.Items.Add(new ListItem("Semester 1", "1"));
            ddlSemester.Items.Add(new ListItem("Semester 2", "2"));
            // Add more semesters as needed
        }

        private void LoadAttendanceData()
        {
            string query = "SELECT StudentID, FormattedRollNo, Name, Gender, Status, Date FROM Attendance";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    gvAttendanceReport.DataSource = dt;
                    gvAttendanceReport.DataBind();
                }
            }
        }

        protected void DdlSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterAttendanceData();
        }

        protected void TxtDate_TextChanged(object sender, EventArgs e)
        {
            FilterAttendanceData();
        }

        private void FilterAttendanceData()
        {
            string query = "SELECT StudentID, FormattedRollNo, Name, Gender, Status, Date FROM Attendance WHERE 1=1";

            if (!string.IsNullOrEmpty(ddlSemester.SelectedValue))
            {
                query += " AND SemesterID = @SemesterID";
            }

            if (!string.IsNullOrEmpty(txtDate.Text))
            {
                query += " AND CONVERT(DATE, Date) = @Date";
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (!string.IsNullOrEmpty(ddlSemester.SelectedValue))
                    {
                        cmd.Parameters.AddWithValue("@SemesterID", ddlSemester.SelectedValue);
                    }

                    if (!string.IsNullOrEmpty(txtDate.Text))
                    {
                        cmd.Parameters.AddWithValue("@Date", txtDate.Text);
                    }

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    gvAttendanceReport.DataSource = dt;
                    gvAttendanceReport.DataBind();
                }
            }
        }
    }
}