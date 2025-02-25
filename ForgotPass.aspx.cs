using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web.UI;
using System.Web.Services;

namespace AMS
{
    public partial class ForgotPass : Page
    {
        string connString = ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;

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

        // Retrieve the user ID from the Users table based on the email
        string userIdQuery = @"
            SELECT id FROM Users WHERE username = @Email";

        int userId = 0;
        using (SqlCommand cmd = new SqlCommand(userIdQuery, conn))
        {
            cmd.Parameters.AddWithValue("@Email", email);
            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                userId = (int)result;
            }
        }

        // If user ID is not found, check the Students and Teachers tables
        if (userId == 0)
        {
            string studentIdQuery = @"
                SELECT id FROM Students WHERE email = @Email";
            using (SqlCommand cmd = new SqlCommand(studentIdQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int studentId = (int)result;
                    // Retrieve the user ID from the Users table based on the student ID
                    userIdQuery = @"
                        SELECT id FROM Users WHERE student_id = @StudentId";
                    using (SqlCommand cmdUser = new SqlCommand(userIdQuery, conn))
                    {
                        cmdUser.Parameters.AddWithValue("@StudentId", studentId);
                        result = cmdUser.ExecuteScalar();
                        if (result != null)
                        {
                            userId = (int)result;
                        }
                    }
                }
            }
        }

        if (userId == 0)
        {
            string teacherIdQuery = @"
                SELECT TeacherID FROM Teachers WHERE Email = @Email";
            using (SqlCommand cmd = new SqlCommand(teacherIdQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int teacherId = (int)result;
                    // Retrieve the user ID from the Users table based on the teacher ID
                    userIdQuery = @"
                        SELECT id FROM Users WHERE teacher_id = @TeacherId";
                    using (SqlCommand cmdUser = new SqlCommand(userIdQuery, conn))
                    {
                        cmdUser.Parameters.AddWithValue("@TeacherId", teacherId);
                        result = cmdUser.ExecuteScalar();
                        if (result != null)
                        {
                            userId = (int)result;
                        }
                    }
                }
            }
        }

        if (userId == 0)
        {
            throw new Exception($"User with email '{email}' not found.");
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

        private void SendOTPEmail(string email, string otp)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("ucspstudentsaffair@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Password Reset OTP code";
                mail.Body = $"Your OTP code is {otp}. OTP expires after 10 minutes.";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("ucspstudentsaffair@gmail.com", "hlso ftks lrbl pzgy");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to send OTP email: " + ex.Message);
            }
        }

        [WebMethod]
        public static string SendOTP(string email)
        {
            ForgotPass instance = new ForgotPass();
            string otp = instance.GenerateOTP();
            DateTime expiry = DateTime.Now.AddMinutes(10); // OTP expires in 10 minutes

            try
            {
                instance.StoreOTPInDatabase(email, otp, expiry);
                instance.SendOTPEmail(email, otp);
                return "OTP sent to your email.";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        [WebMethod]
        public static string VerifyOTP(string email, string otp)
        {
            ForgotPass instance = new ForgotPass();
            if (instance.ValidateOTP(email, otp))
            {
                return "OTP verified";
            }
            else
            {
                return "Invalid or expired OTP";
            }
        }

        [WebMethod]
        public static string UpdatePassword(string email, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                return "Passwords do not match";
            }

            ForgotPass instance = new ForgotPass();
            try
            {
                instance.ChangePassword(email, newPassword);
                return "Password updated successfully";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        private bool ValidateOTP(string email, string otp)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE username = @Email AND OTP = @OTP AND OTPExpiryDateTime > @CurrentDateTime";

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

        private void ChangePassword(string email, string newPassword)
{
    using (SqlConnection conn = new SqlConnection(connString))
    {
        conn.Open();

        // Retrieve the user ID from the Users table based on the email
        string userIdQuery = @"
            SELECT id FROM Users WHERE username = @Email";

        int userId = 0;
        using (SqlCommand cmd = new SqlCommand(userIdQuery, conn))
        {
            cmd.Parameters.AddWithValue("@Email", email);
            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                userId = (int)result;
            }
        }

        // If user ID is not found, check the Students and Teachers tables
        if (userId == 0)
        {
            string studentIdQuery = @"
                SELECT id FROM Students WHERE email = @Email";
            using (SqlCommand cmd = new SqlCommand(studentIdQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int studentId = (int)result;
                    // Retrieve the user ID from the Users table based on the student ID
                    userIdQuery = @"
                        SELECT id FROM Users WHERE student_id = @StudentId";
                    using (SqlCommand cmdUser = new SqlCommand(userIdQuery, conn))
                    {
                        cmdUser.Parameters.AddWithValue("@StudentId", studentId);
                        result = cmdUser.ExecuteScalar();
                        if (result != null)
                        {
                            userId = (int)result;
                        }
                    }
                }
            }
        }

        if (userId == 0)
        {
            string teacherIdQuery = @"
                SELECT TeacherID FROM Teachers WHERE Email = @Email";
            using (SqlCommand cmd = new SqlCommand(teacherIdQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int teacherId = (int)result;
                    // Retrieve the user ID from the Users table based on the teacher ID
                    userIdQuery = @"
                        SELECT id FROM Users WHERE teacher_id = @TeacherId";
                    using (SqlCommand cmdUser = new SqlCommand(userIdQuery, conn))
                    {
                        cmdUser.Parameters.AddWithValue("@TeacherId", teacherId);
                        result = cmdUser.ExecuteScalar();
                        if (result != null)
                        {
                            userId = (int)result;
                        }
                    }
                }
            }
        }

        if (userId == 0)
        {
            throw new Exception("User not found.");
        }

        // Update the Users table with the new password
        string query = "UPDATE Users SET password = @Password, OTP = NULL, OTPExpiryDateTime = NULL WHERE id = @UserId";
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@Password", newPassword);
            cmd.ExecuteNonQuery();
        }
    }
}

        private void showError(string message)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showError", $"showError('{message}');", true);
        }

        private void showSuccess(string message)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccess", $"showSuccess('{message}');", true);
        }
    }
}
