using System;
using System.Web;
using System.Web.UI;

namespace AMS
{
    public partial class students : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if the user is logged in and is a student
                if (Session["UserId"] == null || Session["Role"] == null || Session["Role"].ToString() != "Student")
                {
                    // Redirect to login page if not logged in or not a student
                    Response.Redirect("index.aspx");
                }

                // Display the username
                {
                    // Set the username in the literal control
                    litUsername.Text = Session["Username"] != null ? Session["Username"].ToString() : "Guest";
                }
            }
        }
        
         protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear the session
            Session.Clear();
            Session.Abandon();

            // Set a flag in the session to indicate successful logout
            Session["LogoutSuccess"] = true;

            // Redirect to the login page
            Response.Redirect("index.aspx");
        }
    }
}
