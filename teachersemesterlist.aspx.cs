using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS
{
    public partial class teachersemesterlist : System.Web.UI.Page
    {
        protected Repeater rptSemesters;
        protected GridView gvStudents;
        protected Panel studentsTable;
        protected Label lblTotalPresent;
        protected Label lblTotalAbsence;
        protected Label lblTotalMale;
        protected Label lblTotalFemale;
        protected Label lblTotalStudents;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["TeacherId"] != null)
                {
                    LoadSemesters();
                }
                else
                {
                    Response.Write("<script>alert('Session is NULL. Redirecting to login.');</script>");
                    Response.Redirect("index.aspx", true);
                }
            }
            else
            {
                UpdateAttendanceSummary(); // Update summary on postback
            }
        }

        private void LoadSemesters()
        {
            int teacherId = Convert.ToInt32(Session["TeacherId"]);
            string query = "SELECT DISTINCT s.semester_id, s.semester_name " +
                           "FROM TeacherSubjects ts " +
                           "INNER JOIN Semesters s ON ts.semester_id = s.semester_id " +
                           "WHERE ts.teacher_id = @teacherId";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@teacherId", teacherId);
                    con.Open();
                    rptSemesters.DataSource = cmd.ExecuteReader();
                    rptSemesters.DataBind();
                }
            }
        }

        protected void btnSemester_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int semesterId = Convert.ToInt32(btn.CommandArgument);
            LoadStudents(semesterId);
        }

        private void LoadStudents(int semesterId)
        {
            string query = "SELECT id, formatted_rollno, name, gender FROM Students WHERE semester_id = @semesterId AND status = 'Active'";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@semesterId", semesterId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    gvStudents.DataSource = dt;
                    gvStudents.DataBind();
                    studentsTable.Visible = true;

                    // Update total students label
                    lblTotalStudents.Text = "Total Students: " + dt.Rows.Count;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataTable attendanceData = new DataTable();
            attendanceData.Columns.Add("StudentID", typeof(int));
            attendanceData.Columns.Add("FormattedRollNo", typeof(string));
            attendanceData.Columns.Add("Name", typeof(string));
            attendanceData.Columns.Add("Gender", typeof(string));
            attendanceData.Columns.Add("Status", typeof(string));
            attendanceData.Columns.Add("Date", typeof(DateTime));

            foreach (GridViewRow row in gvStudents.Rows)
            {
                int studentId = Convert.ToInt32(gvStudents.DataKeys[row.RowIndex].Value);
                string formattedRollNo = ((Label)row.FindControl("lblFormattedRollNo")).Text;
                string name = ((Label)row.FindControl("lblName")).Text;
                string gender = ((Label)row.FindControl("lblGender")).Text;
                CheckBox chkPresent = (CheckBox)row.FindControl("chkPresent");
                CheckBox chkAbsence = (CheckBox)row.FindControl("chkAbsence");

                string status = chkPresent.Checked ? "Present" : (chkAbsence.Checked ? "Absence" : "Pending");
                attendanceData.Rows.Add(studentId, formattedRollNo, name, gender, status, DateTime.Now);
            }

            // Save attendance data to database
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    foreach (DataRow row in attendanceData.Rows)
                    {
                        string query = @"
                            IF NOT EXISTS (SELECT 1 FROM Attendance WHERE StudentID = @StudentID AND Date = @Date)
                            BEGIN
                                INSERT INTO Attendance (StudentID, FormattedRollNo, Name, Gender, Status, Date)
                                VALUES (@StudentID, @FormattedRollNo, @Name, @Gender, @Status, @Date)
                            END";
                        using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@StudentID", row["StudentID"]);
                            cmd.Parameters.AddWithValue("@FormattedRollNo", row["FormattedRollNo"]);
                            cmd.Parameters.AddWithValue("@Name", row["Name"]);
                            cmd.Parameters.AddWithValue("@Gender", row["Gender"]);
                            cmd.Parameters.AddWithValue("@Status", row["Status"]);
                            cmd.Parameters.AddWithValue("@Date", row["Date"]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMessage", $"alert('Error: {ex.Message}');", true);
                    return;
                }
            }

            // Show success message
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SuccessMessage", "showSuccessMessage();", true);

            // Redirect to AttendanceReports.aspx
            Response.Redirect("AttendanceReports.aspx");
        }

        protected void chkPresent_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkPresent = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chkPresent.NamingContainer;
            CheckBox chkAbsence = (CheckBox)row.FindControl("chkAbsence");

            if (chkPresent.Checked)
            {
                chkAbsence.Checked = false;
            }

            UpdateStatusLabel(row);
            UpdateAttendanceSummary(); // Update summary
        }

        protected void chkAbsence_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkAbsence = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chkAbsence.NamingContainer;
            CheckBox chkPresent = (CheckBox)row.FindControl("chkPresent");

            if (chkAbsence.Checked)
            {
                chkPresent.Checked = false;
            }

            UpdateStatusLabel(row);
            UpdateAttendanceSummary(); // Update summary
        }

        private void UpdateAttendanceSummary()
        {
            int totalPresent = 0;
            int totalAbsence = 0;
            int totalMale = 0;
            int totalFemale = 0;

            foreach (GridViewRow row in gvStudents.Rows)
            {
                CheckBox chkPresent = (CheckBox)row.FindControl("chkPresent");
                CheckBox chkAbsence = (CheckBox)row.FindControl("chkAbsence");
                Label lblGender = (Label)row.FindControl("lblGender");

                if (chkPresent != null && chkAbsence != null && lblGender != null)
                {
                    if (chkPresent.Checked)
                    {
                        totalPresent++;
                    }
                    if (chkAbsence.Checked)
                    {
                        totalAbsence++;
                    }
                    if (lblGender.Text == "Male")
                    {
                        totalMale++;
                    }
                    if (lblGender.Text == "Female")
                    {
                        totalFemale++;
                    }
                }
            }

            // Update labels
            lblTotalPresent.Text = "Total Present: " + totalPresent;
            lblTotalAbsence.Text = "Total Absence: " + totalAbsence;
            lblTotalMale.Text = "Total Male: " + totalMale;
            lblTotalFemale.Text = "Total Female: " + totalFemale;
            lblTotalStudents.Text = "Total Students: " + (totalPresent + totalAbsence);
        }

        protected void gvStudents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                UpdateStatusLabel(e.Row);
            }
        }

        private void UpdateStatusLabel(GridViewRow row)
        {
            CheckBox chkPresent = (CheckBox)row.FindControl("chkPresent");
            CheckBox chkAbsence = (CheckBox)row.FindControl("chkAbsence");
            Label lblStatus = (Label)row.FindControl("lblStatus");

            if (chkPresent.Checked)
            {
                lblStatus.Text = "Present";
            }
            else if (chkAbsence.Checked)
            {
                lblStatus.Text = "Absence";
            }
            else
            {
                lblStatus.Text = "Pending";
            }
        }
    }
}