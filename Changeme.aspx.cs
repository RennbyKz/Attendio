using System;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI;

namespace AMS
{
    public partial class Changeme : Page
    {
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;

        [WebMethod]
        public static string CheckUsername(string username)
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE username = @Username";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0 ? "taken" : "available";
                }
            }
        }
        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.Value != txtConfirmPassword.Value)
            {
                ShowSweetAlert("Error!", "Passwords do not match!", "error");
                return;
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            string newUsername = textNewUsername.Value.Trim();
            string newPassword = txtNewPassword.Value.Trim();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Check if the new username is already taken by another user
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE username = @Username AND id <> @UserId";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Username", newUsername);
                    checkCmd.Parameters.AddWithValue("@UserId", userId);

                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        // Username is already taken, generate a unique one
                        newUsername = GenerateUniqueUsername(conn, newUsername);
                    }
                }

                // Update username and password
                string updateQuery = "UPDATE Users SET username = @Username, password = @Password, is_first_login = 0 WHERE id = @UserId";
                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", newUsername);
                    cmd.Parameters.AddWithValue("@Password", newPassword); // ⚠️ Password is stored as plain text (not recommended)
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        ShowSweetAlertRedirect("Success!", "Password changed successfully!", "success", "Index.aspx");
                    }
                    else
                    {
                        ShowSweetAlert("Error!", "Error updating password!", "error");
                    }
                }
            }
        }

        // ✅ Generate a unique username by appending a numeric suffix
        private string GenerateUniqueUsername(SqlConnection conn, string baseUsername)
        {
            int count = 1;
            string newUsername = baseUsername;
            string query = "SELECT COUNT(*) FROM Users WHERE username = @Username";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", newUsername);

                while ((int)cmd.ExecuteScalar() > 0)
                {
                    newUsername = baseUsername + "_" + count;
                    cmd.Parameters["@Username"].Value = newUsername;
                    count++;
                }
            }
            return newUsername;
        }

        private void ShowSweetAlert(string title, string message, string icon)
        {
            string script = $"Swal.fire('{title}', '{message}', '{icon}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", script, true);
        }

        private void ShowSweetAlertRedirect(string title, string message, string icon, string redirectUrl)
        {
            string script = $"Swal.fire({{ title: '{title}', text: '{message}', icon: '{icon}', confirmButtonText: 'OK' }}).then(function() {{ window.location.href = '{redirectUrl}'; }});";
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", script, true);
        }
    }
}