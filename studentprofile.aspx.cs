using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace AMS
{
    public partial class studentprofile : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["StudentId"] != null)
                {
                    LoadStudentProfile();
                }
                else
                {
                    // Redirect to login page if session data is missing
                    Response.Redirect("index.aspx");
                }
            }
        }

        private void LoadStudentProfile()
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;
            int studentId = Convert.ToInt32(Session["StudentId"]);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT s.name, sem.semester_name, s.formatted_rollno, s.section, s.gender, s.email, s.major, s.profile_picture " +
                               "FROM Students s " +
                               "JOIN Semesters sem ON s.semester_id = sem.semester_id " +
                               "WHERE s.id = @StudentId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentId", studentId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblName.Text = reader["name"].ToString();
                    lblSemester.Text = reader["semester_name"].ToString();
                    lblRollNo.Text = reader["formatted_rollno"].ToString();
                    lblSection.Text = reader["section"].ToString();
                    lblGender.Text = reader["gender"].ToString();
                    lblEmail.Text = reader["email"].ToString();
                    lblMajor.Text = reader["major"].ToString();

                    string profilePicture = reader["profile_picture"].ToString();
                    if (!string.IsNullOrEmpty(profilePicture))
                    {
                        imgProfile.ImageUrl = "~/studentsphotos/" + profilePicture;
                    }
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fileUpload.HasFile)
            {
                string fileName = System.IO.Path.GetFileName(fileUpload.PostedFile.FileName);
                string filePath = Server.MapPath("~/studentsphotos/") + fileName;
                fileUpload.SaveAs(filePath);

                // Update the profile picture path in the database
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;
                int studentId = Convert.ToInt32(Session["StudentId"]);

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "UPDATE Students SET profile_picture = @ProfilePicture WHERE id = @StudentId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProfilePicture", fileName);
                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                // Refresh the profile picture
                LoadStudentProfile();
            }
            else
            {
                // Show alert if no file is selected
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Please select a profile picture.');", true);
            }
        }
    }
}
