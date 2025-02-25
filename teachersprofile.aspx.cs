using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls; // Add this line
using System.IO;

namespace AMS
{
    public partial class teachersprofile : Page
    {
        protected Label lblRoom; // Add this line to declare lblRoom

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["TeacherId"] != null)
                {
                    LoadTeacherProfile();
                }
                else
                {
                    Response.Write("<script>alert('Session is NULL. Redirecting to login.');</script>");
                    Response.Redirect("index.aspx", true);
                }
            }
        }

        private void LoadTeacherProfile()
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;
            int teacherId = Convert.ToInt32(Session["TeacherId"]);

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        t.FullName, 
                        t.Email, 
                        t.PhoneNumber, 
                        d.departmentname, 
                        d.room, 
                        t.status, 
                        t.ProfileImage 
                    FROM 
                        Teachers t
                    JOIN 
                        Departments d ON t.department_id = d.departmentId 
                    WHERE 
                        t.TeacherID = @TeacherID", con);
                cmd.Parameters.AddWithValue("@TeacherID", teacherId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    lblFullName.Text = reader["FullName"].ToString();
                    lblEmail.Text = reader["Email"].ToString();
                    lblPhoneNumber.Text = reader["PhoneNumber"].ToString();
                    lblDepartment.Text = reader["departmentname"].ToString();
                    lblRoom.Text = reader["room"].ToString();
                    lblStatus.Text = reader["status"].ToString();
                    imgProfile.ImageUrl = reader["ProfileImage"] != DBNull.Value ? reader["ProfileImage"].ToString() : "~/images/default-profile.png";
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fileUpload.HasFile)
            {
                try
                {
                    string fileName = Path.GetFileName(fileUpload.PostedFile.FileName);
                    string directoryPath = Server.MapPath("~/teachersphotos/");
                    string filePath = Path.Combine(directoryPath, fileName);

                    // Ensure the directory exists
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    fileUpload.SaveAs(filePath);

                    // Update the profile picture path in the database
                    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;
                    int teacherId = Convert.ToInt32(Session["TeacherId"]);

                    using (SqlConnection con = new SqlConnection(connString))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("UPDATE Teachers SET ProfileImage = @ProfileImage WHERE TeacherID = @TeacherID", con);
                        cmd.Parameters.AddWithValue("@ProfileImage", "~/teachersphotos/" + fileName);
                        cmd.Parameters.AddWithValue("@TeacherID", teacherId);
                        cmd.ExecuteNonQuery();
                    }

                    // Refresh the profile image
                    LoadTeacherProfile();
                }
                catch (Exception ex)
                {
                    // Handle the error
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"Swal.fire({{title: 'Error!', text: 'Error: {ex.Message}', icon: 'error', confirmButtonText: 'OK'}});", true);
                }
            }
            else
            {
                // Show alert if no file is selected
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "Swal.fire({title: 'Alert!', text: 'Please select a profile picture.', icon: 'warning', confirmButtonText: 'OK'});", true);
            }
        }
    }
}