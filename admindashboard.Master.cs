using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS
{
    public partial class admindashboard : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if the user is logged in and is an admin
                if (Session["UserId"] == null || Session["Role"] == null || Session["Role"].ToString() != "Admin")
                {
                    // Redirect to login page if not logged in or not an admin
                    Response.Redirect("index.aspx");
                }

                // Display the username
                litUsername.Text = Session["Username"] != null ? Session["Username"].ToString() : "Guest";
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



