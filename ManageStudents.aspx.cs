using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace AMS
{
    public partial class ManageStudents : System.Web.UI.Page
    {
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSemesters();
                LoadStudents();
            }
        }

        private void LoadSemesters()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT semester_id, semester_name, year_name FROM Semesters", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlSemester.DataSource = dt;
                ddlSemester.DataTextField = "semester_name";
                ddlSemester.DataValueField = "semester_id";
                ddlSemester.DataBind();
                ddlSemester.Items.Insert(0, new ListItem("All Semesters", "0"));
            }
        }

        private void LoadStudents(string searchQuery = "", int semesterId = 0)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"SELECT s.id, s.rollno, s.formatted_rollno, s.name, 
                                s.gender, s.email, s.major, s.section, s.status, 
                                sem.semester_id, sem.semester_name, sem.year_name 
                         FROM Students s 
                         JOIN Semesters sem ON s.semester_id = sem.semester_id 
                         WHERE (@SearchQuery = '' OR s.name LIKE @SearchQuery) 
                         AND (@SemesterId = 0 OR s.semester_id = @SemesterId)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchQuery", string.IsNullOrEmpty(searchQuery) ? "" : "%" + searchQuery + "%");
                cmd.Parameters.AddWithValue("@SemesterId", semesterId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvStudents.DataSource = dt;
                gvStudents.DataBind();

                // Calculate and display total users
                lblTotalUsers.Text = dt.Rows.Count.ToString();
            }
        }

        // Add student event
        protected void btnSaveStudent_Click(object sender, EventArgs e)
        {
            string rollNo = txtRollNoModal.Text;
            string name = txtNameModal.Text;
            string gender = ddlGenderModal.SelectedValue;
            string email = txtEmailModal.Text;
            string major = ddlMajorModal.SelectedValue;
            string section = ddlSectionModal.SelectedValue;
            int semesterId = int.Parse(ddlSemesterModal.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Students (rollno, name, gender, email, major, section, semester_id) VALUES (@rollno, @name, @gender, @email, @major, @section, @semester_id)", conn);
                cmd.Parameters.AddWithValue("@rollno", rollNo);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@major", major);
                cmd.Parameters.AddWithValue("@section", section);
                cmd.Parameters.AddWithValue("@semester_id", semesterId);
                cmd.ExecuteNonQuery();
            }

            LoadStudents();
            CloseModal();

            // Show success message
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('Student added successfully');", true);
        }


        private void CloseModal()
        {
            // Implementation to close the modal
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadStudents(txtSearch.Text, int.Parse(ddlSemester.SelectedValue));
        }

        protected void ddlSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudents(txtSearch.Text, int.Parse(ddlSemester.SelectedValue));
        }

        protected void gvStudents_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvStudents.EditIndex = e.NewEditIndex;
            LoadStudents();
        }

        protected void gvStudents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvStudents.EditIndex = -1;
            LoadStudents();
        }

        protected void gvStudents_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvStudents.Rows[e.RowIndex];
            int id = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);
            int rollno = Convert.ToInt32((row.Cells[1].Controls[0] as TextBox).Text);
            string name = (row.Cells[3].Controls[0] as TextBox).Text;

            // Get values from DropDownLists
            DropDownList ddlGender = (DropDownList)row.FindControl("ddlGender");
            string gender = ddlGender.SelectedValue;

            DropDownList ddlMajor = (DropDownList)row.FindControl("ddlMajor");
            string major = ddlMajor.SelectedValue;

            DropDownList ddlSection = (DropDownList)row.FindControl("ddlSection");
            string section = ddlSection.SelectedValue;

            DropDownList ddlSemesterId = (DropDownList)row.FindControl("ddlSemesterId");
            int semesterId = Convert.ToInt32(ddlSemesterId.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Students SET rollno=@Rollno, name=@Name, gender=@Gender, major=@Major, section=@Section, semester_id=@SemesterId WHERE id=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Rollno", rollno);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Major", major);
                cmd.Parameters.AddWithValue("@Section", section);
                cmd.Parameters.AddWithValue("@SemesterId", semesterId);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            gvStudents.EditIndex = -1;
            LoadStudents();
        }

        protected void gvStudents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                // Bind Gender DropDownList
                DropDownList ddlGender = (DropDownList)e.Row.FindControl("ddlGender");
                string gender = DataBinder.Eval(e.Row.DataItem, "gender").ToString();
                ddlGender.SelectedValue = gender;

                // Bind Major DropDownList
                DropDownList ddlMajor = (DropDownList)e.Row.FindControl("ddlMajor");
                string major = DataBinder.Eval(e.Row.DataItem, "major").ToString();
                ddlMajor.SelectedValue = major;

                // Bind Section DropDownList
                DropDownList ddlSection = (DropDownList)e.Row.FindControl("ddlSection");
                string section = DataBinder.Eval(e.Row.DataItem, "section").ToString();
                ddlSection.SelectedValue = section;

                // Bind Semester ID DropDownList
                DropDownList ddlSemesterId = (DropDownList)e.Row.FindControl("ddlSemesterId");
                string semesterId = DataBinder.Eval(e.Row.DataItem, "semester_id").ToString();
                ddlSemesterId.SelectedValue = semesterId;
            }
        }
        protected void gvStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "DELETE FROM Students WHERE id=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();

                // Reseed identity column so the next student gets the deleted ID.
                string reseedQuery = "DBCC CHECKIDENT ('Students', RESEED, @Id)";
                SqlCommand reseedCmd = new SqlCommand(reseedQuery, conn);
                reseedCmd.Parameters.AddWithValue("@Id", id);

                reseedCmd.ExecuteNonQuery();
            }

            LoadStudents();

            // Show success message
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('Student deleted successfully');", true);
        }


    }
}