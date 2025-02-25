using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace AMS
{
    public partial class index : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["LogoutSuccess"] != null && (bool)Session["LogoutSuccess"])
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                        "Swal.fire({text: 'Logged out successfully.', icon: 'success', confirmButtonText: 'OK'});", true);
                    Session["LogoutSuccess"] = null;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                    "Swal.fire({title: 'Alert!', text: 'Please enter username and password.', icon: 'warning', confirmButtonText: 'OK'});", true);
                return;
            }

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT id, role, is_first_login, student_id, teacher_id FROM Users WHERE Username = @Username AND Password = @Password", con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int userId = Convert.ToInt32(reader["id"]);
                    string role = reader["role"].ToString();
                    bool isFirstLogin = Convert.ToBoolean(reader["is_first_login"]);
                    int? studentId = reader["student_id"] as int?;
                    int? teacherId = reader["teacher_id"] as int?;

                    // Store session data
                    Session["UserId"] = userId;
                    Session["Username"] = username;
                    Session["Role"] = role;

                    // Additional session data for students and teachers
                    if (studentId.HasValue)
                    {
                        Session["StudentId"] = studentId.Value;
                    }
                    if (teacherId.HasValue)
                    {
                        Session["TeacherId"] = teacherId.Value;
                    }

                    string redirectUrl;
                    if (isFirstLogin)
                    {
                        redirectUrl = "Changeme.aspx";
                    }
                    else if (role == "Admin")
                    {
                        redirectUrl = "Dashboard.aspx";
                    }
                    else if (role == "Teacher")
                    {
                        if (teacherId.HasValue)
                        {
                            redirectUrl = "teachersemesterlist.aspx";
                        }
                        else
                        {
                            // Handle case where teacher ID is not set
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                                "Swal.fire({title: 'Error!', text: 'Teacher ID not found.', icon: 'error', confirmButtonText: 'OK'});", true);
                            return;
                        }
                    }
                    else if (role == "Student")
                    {
                        if (studentId.HasValue)
                        {
                            redirectUrl = "studentinbox.aspx";
                        }
                        else
                        {
                            // Handle case where student ID is not set
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                                "Swal.fire({title: 'Error!', text: 'Student ID not found.', icon: 'error', confirmButtonText: 'OK'});", true);
                            return;
                        }
                    }
                    else
                    {
                        redirectUrl = "";
                    }

                    // Show SweetAlert2 for successful login and redirect
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                        $"Swal.fire({{title: 'Success!', text: 'Login successful.', icon: 'success', confirmButtonText: 'OK'}}).then(function() {{ window.location.href = '{redirectUrl}'; }});", true);
                }
                else
                {
                    // Show error alert
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                        "Swal.fire({title: 'Error!', text: 'Invalid username or password.', icon: 'error', confirmButtonText: 'OK'});", true);
                }
            }
        }
    }
}


