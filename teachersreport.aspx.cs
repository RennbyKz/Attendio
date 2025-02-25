using System;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;

namespace AMS
{
    public partial class teachersreport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // Prevents loading data multiple times
            {
                LoadTeacherProfile();
            }
        }

        private void LoadTeacherProfile()
        {
            string connString = WebConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;

            // Ensure TeacherId exists in session
            if (Session["TeacherId"] == null)
            {
                Response.Redirect("Login.aspx"); // Redirect to login if session is null
                return;
            }

            int teacherId = Convert.ToInt32(Session["TeacherId"]);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT t.FullName, t.Email, t.role, dep.departmentname " +
                               "FROM Teachers t " +
                               "JOIN Departments dep ON t.department_id = dep.departmentId " +
                               "WHERE t.TeacherID = @TeacherId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TeacherId", teacherId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtName.Text = reader["FullName"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtRole.Text = reader["role"].ToString();
                    txtDep.Text = reader["departmentname"].ToString();
                }
                reader.Close();
            }
        }

       protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("ucspstudentsaffair@gmail.com");

                // Admin email address
                mail.To.Add("ucspstudentsaffair@gmail.com");
                mail.Subject = txtSubject.Text;

                mail.Body = "Name: " + txtName.Text + "<br/>" +
                            "Email: " + txtEmail.Text + "<br/>" +
                            "Role: " + txtRole.Text + "<br/>" +
                            "Department: " + txtDep.Text + "<br/>" +
                            "Message: " + txtMessage.Text;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587; // Gmail SMTP port
                smtp.Credentials = new System.Net.NetworkCredential("ucspstudentsaffair@gmail.com", "hlso ftks lrbl pzgy"); // Admin email credentials
                smtp.EnableSsl = true;
                smtp.Send(mail);

                // Display success message
                string script = "Swal.fire('Success', 'Mail Sent Successfully!', 'success');";
                ScriptManager.RegisterStartupScript(this, GetType(), "Alert", script, true);
            }
            catch (Exception ex)
            {
                string script = $"Swal.fire('Error', 'Error: {ex.Message}', 'error');";
                ScriptManager.RegisterStartupScript(this, GetType(), "Alert", script, true);
            }
        }
    }
}
