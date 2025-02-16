using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        protected void LoadUsers()
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = @"
            SELECT u.id, u.username, u.role, u.Created_at,
                COALESCE(s.name, t.FullName) AS RelatedName
            FROM Users u
            LEFT JOIN Students s ON u.student_id = s.id
            LEFT JOIN Teachers t ON u.teacher_id = t.TeacherID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvUsers.DataSource = dt;
                    gvUsers.DataBind();

                    // Calculate and display total users
                    lblTotalUsers.Text = dt.Rows.Count.ToString();

                }
            }
        }



      protected void btnSaveUser_Click(object sender, EventArgs e)
{
    string baseUsername = txtUsernameModal.Text.ToLower().Replace(" ", "");
    string password = txtPasswordModal.Text;
    string name = textnameModal.Text; // Ensure correct ID is used
    string role = ddlRoleModal.SelectedValue;
    string newUsername = baseUsername;
    int suffix = 1;

    string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;
    using (SqlConnection conn = new SqlConnection(connString))
    {
        conn.Open();

        // Debugging: Check if name is fetched correctly
      //  lblMessage.Text = "Name: " + name + "<br />";

        // Check if username exists and generate unique username
        while (true)
        {
            string checkUsernameQuery = "SELECT COUNT(*) FROM Users WHERE username = @username";
            using (SqlCommand checkCmd = new SqlCommand(checkUsernameQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("@username", newUsername);
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    break; // Unique username found
                }
                else
                {
                    newUsername = baseUsername + suffix.ToString();
                    suffix++;
                }
            }
        }

        // Insert new user with unique username
        string query = "INSERT INTO Users (username, password, role, RelatedName, is_first_login) VALUES (@username, @password, @role, @name, 0)";
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@username", newUsername);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@role", role);
            cmd.Parameters.AddWithValue("@name", name);

            try
            {
                cmd.ExecuteNonQuery();
                LoadUsers();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", "closeModal();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('New Admin added successfully!');", true);
            }
            catch (SqlException ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorMessage", "showErrorMessage('Error: " + ex.Message + "');", true);
            }
        }
    }
}



        protected void btnGenerateStudents_Click(object sender, EventArgs e)
        {
            InsertActiveStudents();
            LoadUsers();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('Students generated successfully!');", true);
        }

        protected void btnGenerateTeachers_Click(object sender, EventArgs e)
        {
            InsertActiveTeachers();  
            LoadUsers();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('Teachers generated successfully!');", true);
        }

        protected void InsertActiveStudents()
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Insert active students with unique usernames
                string studentQuery = @"
        DECLARE @baseUsername NVARCHAR(50), @newUsername NVARCHAR(50), @suffix INT, @student_id INT;

        DECLARE studentCursor CURSOR FOR
        SELECT s.name, s.id
        FROM Students s
        WHERE s.status = 'Active'
        AND NOT EXISTS (
            SELECT 1 FROM Users u 
            WHERE u.student_id = s.id
            AND u.role = 'Student');

        OPEN studentCursor;
        FETCH NEXT FROM studentCursor INTO @baseUsername, @student_id;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            SET @baseUsername = LOWER(REPLACE(@baseUsername, ' ', ''));
            SET @newUsername = @baseUsername;
            SET @suffix = 1;

            WHILE EXISTS (SELECT 1 FROM Users u WHERE u.username = @newUsername)
            BEGIN
                SET @newUsername = @baseUsername + CAST(@suffix AS NVARCHAR);
                SET @suffix = @suffix + 1;
            END

            INSERT INTO Users (username, password, role, is_first_login, student_id)
            VALUES (@newUsername, 'changeme', 'Student', 1, @student_id);

            FETCH NEXT FROM studentCursor INTO @baseUsername, @student_id;
        END

        CLOSE studentCursor;
        DEALLOCATE studentCursor;";

                using (SqlCommand cmd = new SqlCommand(studentQuery, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        // lblMessage.Text = "Students inserted successfully!";
                    }
                    catch (SqlException ex)
                    {
                        lblMessage.Text += "Error inserting students: " + ex.Message + "<br />";
                    }
                }
            }
        }


        protected void InsertActiveTeachers()
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Insert active teachers
                string teacherQuery = @"
        INSERT INTO Users (username, password, role, is_first_login, teacher_id)
        SELECT 
            LOWER(REPLACE(t.FullName, ' ', '')) +
            COALESCE(
                (SELECT CAST(COUNT(*) + 1 AS NVARCHAR) 
                FROM Users u 
                WHERE u.username LIKE LOWER(REPLACE(t.FullName, ' ', '')) + '%'), 
                ''
            ),
            'changeme',
            'Teacher',
            1,
            t.TeacherID
        FROM Teachers t
        WHERE t.status = 'Active'
        AND NOT EXISTS (
            SELECT 1 FROM Users u 
            WHERE u.teacher_id = t.TeacherID
            AND u.role = 'Teacher')";

                using (SqlCommand cmd = new SqlCommand(teacherQuery, conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        lblMessage.Text += "Error inserting teachers: " + ex.Message;
                    }
                }
            }
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            LoadUsers();
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            LoadUsers();
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
            string username = ((TextBox)gvUsers.Rows[e.RowIndex].FindControl("txtUsername")).Text;
            string role = ((TextBox)gvUsers.Rows[e.RowIndex].FindControl("txtRole")).Text;

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "UPDATE Users SET username = @username, role = @role WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@role", role);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        lblMessage.Text = "User updated successfully!";
                        gvUsers.EditIndex = -1;
                        LoadUsers();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('User updated successfully!');", true);
                    }
                    catch (SqlException ex)
                    {
                        lblMessage.Text = "Error: " + ex.Message;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorMessage", "showErrorMessage('Error: " + ex.Message + "');", true);
                    }
                }
            }
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "DELETE FROM Users WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", userId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                      //  lblMessage.Text = "User deleted successfully!";
                        LoadUsers();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('User deleted successfully!');", true);
                    }
                    catch (SqlException ex)
                    {
                        lblMessage.Text = "Error: " + ex.Message;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorMessage", "showErrorMessage('Error: " + ex.Message + "');", true);
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT * FROM Users WHERE username LIKE '%' + @searchQuery + '%'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@searchQuery", searchQuery);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvUsers.DataSource = dt;
                    gvUsers.DataBind();
                    
                }
            }
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRole = ddlRole.SelectedValue;
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = selectedRole == "All" ? "SELECT * FROM Users" : "SELECT * FROM Users WHERE role = @role";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (selectedRole != "All")
                    {
                        cmd.Parameters.AddWithValue("@role", selectedRole);
                    }
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvUsers.DataSource = dt;
                    gvUsers.DataBind();
                }
            }
        }
    }
}
