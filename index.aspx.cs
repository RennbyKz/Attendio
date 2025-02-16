using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace AMS
{
    public partial class index : Page
    {
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
                SqlCommand cmd = new SqlCommand("SELECT id, role, is_first_login FROM Users WHERE Username = @Username AND Password = @Password", con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int userId = Convert.ToInt32(reader["id"]);
                    string role = reader["role"].ToString();
                    bool isFirstLogin = Convert.ToBoolean(reader["is_first_login"]);

                    // Store session data
                    Session["UserId"] = userId;
                    Session["Username"] = username;
                    Session["Role"] = role;

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
                        redirectUrl = "TeacherDashboard.aspx";
                    }
                    else if (role == "Student")
                    {
                        redirectUrl = "StudentDashboard.aspx";
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

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
