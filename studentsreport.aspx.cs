using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS
{
    public partial class studentsreport : System.Web.UI.Page
    {
        protected string studentEmail;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStudentProfile();
            }
        }

        private void LoadStudentProfile()
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;
            int studentId = Convert.ToInt32(Session["StudentId"]);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT s.name, s.formatted_rollno, s.section, s.email " +
                               "FROM Students s " +
                               "JOIN Semesters sem ON s.semester_id = sem.semester_id " +
                               "WHERE s.id = @StudentId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentId", studentId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtName.Text = reader["name"].ToString();
                    txtRollNo.Text = reader["formatted_rollno"].ToString();
                    txtSection.Text = reader["section"].ToString();
                    txtEmail.Text = reader["email"].ToString(); // This line updates the txtEmail TextBox
               
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("ucspstudentsaffair@gmail.com"); // Use the student's email address

                // Admin email address
                mail.To.Add("ucspstudentsaffair@gmail.com");

                mail.Subject = txtSubject.Text;
                mail.Body = "Name: " + txtName.Text + "<br/>Roll No: " + txtRollNo.Text + "<br/>Section: " + txtSection.Text + "<br/>Email: " + txtEmail.Text + "<br/>Message: " + txtMessage.Text;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587; // Gmail SMTP port
                smtp.Credentials = new System.Net.NetworkCredential("ucspstudentsaffair@gmail.com", "hlso ftks lrbl pzgy"); // Admin email credentials
                smtp.EnableSsl = true;
                smtp.Send(mail);

                // Register SweetAlert success message script
                string script = "Swal.fire('Success', 'Mail Sent Successfully!', 'success');";
                ClientScript.RegisterStartupScript(this.GetType(), "Alert", script, true);
            }
            catch (Exception ex)
            {
                // Register SweetAlert error message script
                string script = $"Swal.fire('Error', 'Error: {ex.Message}', 'error');";
                ClientScript.RegisterStartupScript(this.GetType(), "Alert", script, true);
            }
        }
    }
}
