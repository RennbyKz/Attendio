using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Security.Cryptography;
using System.Text;

namespace AMS
{
    public partial class teachersetting : System.Web.UI.Page
    { string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load current username
                LoadCurrentUsername();
            }
        }

        private void LoadCurrentUsername()
        {
            // Assuming you have a method to get the user ID from the session
            int userId = Convert.ToInt32(Session["UserId"]);

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT username, last_username_change FROM Users WHERE id = @UserId", con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    lblCurrentUsername.Text = "Current Username: " + reader["username"].ToString();
                    ViewState["LastUsernameChange"] = reader["last_username_change"] != DBNull.Value ? (DateTime?)reader["last_username_change"] : null;
                }
            }
        }

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

        protected void btnChangeUsername_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            string newUsername = txtNewUsername.Text.Trim();

            if (string.IsNullOrEmpty(newUsername))
            {
                lblUserMsg.Text = "New username cannot be empty.";
                lblUserMsg.CssClass = "text-danger mt-2";
                return;
            }

            DateTime? lastUsernameChange = ViewState["LastUsernameChange"] as DateTime?;
            if (lastUsernameChange.HasValue && (DateTime.Now - lastUsernameChange.Value).TotalDays < 30)
            {
                lblUserMsg.Text = "You can only change your username once every 30 days.";
                lblUserMsg.CssClass = "text-danger mt-2";
                return;
            }

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Users SET username = @Username, last_username_change = @LastUsernameChange WHERE id = @UserId", con);
                cmd.Parameters.AddWithValue("@Username", newUsername);
                cmd.Parameters.AddWithValue("@LastUsernameChange", DateTime.Now);
                cmd.Parameters.AddWithValue("@UserId", userId);

                cmd.ExecuteNonQuery();
            }

            lblUserMsg.Text = "Username updated successfully.";
            lblUserMsg.CssClass = "text-success mt-2";
            LoadCurrentUsername(); // Refresh the current username display
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            string currentPassword = txtCurrentPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(currentPassword) && string.IsNullOrEmpty(newPassword) && string.IsNullOrEmpty(confirmPassword))
            {
                lblPassMsg.Text = "All password fields are required.";
                lblPassMsg.CssClass = "text-danger mt-2";
                return;
            }

            if (newPassword != confirmPassword)
            {
                lblPassMsg.Text = "New passwords do not match.";
                lblPassMsg.CssClass = "text-danger mt-2";
                return;
            }

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT password FROM Users WHERE id = @UserId", con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    string currentPasswordInDb = reader["password"].ToString();

                    if (currentPassword != currentPasswordInDb)
                    {
                        lblPassMsg.Text = "Current password is incorrect.";
                        lblPassMsg.CssClass = "text-danger mt-2";
                        return;
                    }
                }
            }

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Users SET password = @Password WHERE id = @UserId", con);
                cmd.Parameters.AddWithValue("@Password", newPassword);
                cmd.Parameters.AddWithValue("@UserId", userId);

                cmd.ExecuteNonQuery();
            }

            lblPassMsg.Text = "Password updated successfully.";
            lblPassMsg.CssClass = "text-success mt-2";
        }
    }
}
