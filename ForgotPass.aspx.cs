using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS
{
    public partial class ForgotPass : Page
    {
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;

        protected void btnSendOTP_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            bool emailExists = false;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string studentQuery = "SELECT COUNT(*) FROM Students WHERE email = @Email";
                using (SqlCommand cmd = new SqlCommand(studentQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0) emailExists = true;
                }

                if (!emailExists)
                {
                    string teacherQuery = "SELECT COUNT(*) FROM Teachers WHERE email = @Email";
                    using (SqlCommand cmd = new SqlCommand(teacherQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = (int)cmd.ExecuteScalar();
                        if (count > 0) emailExists = true;
                    }
                }
            }

            if (!emailExists)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "showError('Your Email is Not Registered!');", true);
                return;
            }


            // Generate and Send OTP
            string otp = GenerateOTP();
            DateTime expiry = DateTime.Now.AddMinutes(3);

            StoreOTPInDatabase(email, otp, expiry);
            SendOTPEmail(email, otp);

            Session["Email"] = email;
            ScriptManager.RegisterStartupScript(this, GetType(), "showOTPSection", "showOTPSection();", true);
        }


        protected void btnVerifyOTP_Click(object sender, EventArgs e)
        {
            string email = Session["Email"] as string;
            string enteredOTP = txtOTP.Text.Trim();

            if (ValidateOTP(email, enteredOTP))
            {
                // OTP is correct, show password section
                ScriptManager.RegisterStartupScript(this, GetType(), "showPasswordSection", "showPasswordSection();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "showError('Invalid or expired OTP!');", true);
            }
        }

        protected void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            string email = Session["Email"] as string;
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (newPassword != confirmPassword)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "showError('Passwords do not match!');", true);
                return;
            }

            ChangePassword(email, newPassword);
            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess", "showSuccess('Password changed successfully!');", true);
        }


        private string GenerateOTP()
        {
            Random rand = new Random();
            return rand.Next(100000, 999999).ToString();
        }

        private void StoreOTPInDatabase(string email, string otp, DateTime expiry)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Retrieve the user ID from the Students or Teachers table
                string userIdQuery = @"
                    SELECT id FROM Students WHERE email = @Email
                    UNION
                    SELECT TeacherID FROM Teachers WHERE email = @Email";

                int userId = 0;
                using (SqlCommand cmd = new SqlCommand(userIdQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    userId = (int)cmd.ExecuteScalar();
                }

                if (userId == 0)
                {
                    throw new Exception("User not found.");
                }

                // Update the Users table with the OTP and expiry date
                string query = "UPDATE Users SET OTP = @OTP, OTPExpiryDateTime = @ExpiryDateTime WHERE id = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@OTP", otp);
                    cmd.Parameters.AddWithValue("@ExpiryDateTime", expiry);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private bool ValidateOTP(string email, string otp)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE email = @Email AND OTP = @OTP AND OTPExpiryDateTime > @CurrentDateTime";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@OTP", otp);
                    cmd.Parameters.AddWithValue("@CurrentDateTime", DateTime.Now);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        private void SendOTPEmail(string email, string otp)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("stdntsaffair@gmail.com");
            mail.To.Add(email);
            mail.Subject = "Password Reset OTP code";
            mail.Body = $"Your OTP code is {otp} OTP expire after 3 minutes";

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new NetworkCredential("stdntsaffair@gmail.com", "cynh thpp dcqp twwt");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        private void ChangePassword(string email, string newPassword)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Retrieve the user ID from the Students or Teachers table
                string userIdQuery = @"
                    SELECT UserId FROM Students WHERE email = @Email
                    UNION
                    SELECT UserId FROM Teachers WHERE email = @Email";

                int userId = 0;
                using (SqlCommand cmd = new SqlCommand(userIdQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    userId = (int)cmd.ExecuteScalar();
                }

                if (userId == 0)
                {
                    throw new Exception("User not found.");
                }

                // Update the Users table with the new password
                string query = "UPDATE Users SET Password = @Password WHERE UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Password", newPassword);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}