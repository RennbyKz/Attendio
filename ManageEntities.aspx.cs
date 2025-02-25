using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace AMS
{
    public partial class ManageEntities : System.Web.UI.Page
    {
        string connString = ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSemesters();
                LoadDepartments();
                LoadSubjects();
                ShowSelectedSection(); // Ensure correct section is visible
            }
        }

        protected void ddlTableSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSelectedSection();
        }

        private void ShowSelectedSection()
        {
            string selectedValue = ddlTableSelection.SelectedValue;

            // Hide all sections initially
            semestersSection.Visible = false;
            departmentsSection.Visible = false;
            subjectsSection.Visible = false;

            // Show the selected section
            switch (selectedValue)
            {
                case "Semesters":
                    semestersSection.Visible = true;
                    LoadSemesters();  // Ensure data is loaded
                    break;
                case "Departments":
                    departmentsSection.Visible = true;
                    LoadDepartments();
                    break;
                case "Subjects":
                    subjectsSection.Visible = true;
                    LoadSubjects();
                    break;
            }
        }

        // Methods for managing semesters
        private void LoadSemesters()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT semester_id, semester_name, year_name FROM Semesters", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvSemesters.DataSource = dt;
                gvSemesters.DataBind();
            }
        }

        protected void btnSaveSemester_Click(object sender, EventArgs e)
        {
            string semesterName = txtSemesterName.Text;
            string yearName = txtYearName.Text;

            // Validate length constraints
            if (semesterName.Length > 20) semesterName = semesterName.Substring(0, 20);
            if (yearName.Length > 30) yearName = yearName.Substring(0, 30);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Semesters (semester_name, year_name) VALUES (@semester_name, @year_name)", conn);
                cmd.Parameters.AddWithValue("@semester_name", semesterName);
                cmd.Parameters.AddWithValue("@year_name", yearName);
                cmd.ExecuteNonQuery();
            }

            LoadSemesters();
            CloseModal("semesterModal");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('Semester added successfully');", true);
        }

        // Methods for managing departments
        private void LoadDepartments()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT departmentId, departmentname, room FROM Departments", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvDepartments.DataSource = dt;
                gvDepartments.DataBind();
            }
        }

        protected void btnSaveDepartment_Click(object sender, EventArgs e)
        {
            string departmentName = txtDepartmentName.Text;
            string room = txtDepartmentRoom.Text;

            // Validate length constraints
            if (departmentName.Length > 100) departmentName = departmentName.Substring(0, 100);
            if (room.Length > 10) room = room.Substring(0, 10);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Departments (departmentname, room) VALUES (@departmentname, @room)", conn);
                cmd.Parameters.AddWithValue("@departmentname", departmentName);
                cmd.Parameters.AddWithValue("@room", room);
                cmd.ExecuteNonQuery();
            }

            LoadDepartments();
            CloseModal("departmentModal");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('Department added successfully');", true);
        }

        // Methods for managing subjects
        private void LoadSubjects()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT subject_id, subject_code, subject_name, semester_id, department_id FROM Subjects", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvSubjects.DataSource = dt;
                gvSubjects.DataBind();
            }
        }

        protected void btnSaveSubject_Click(object sender, EventArgs e)
        {
            string subjectCode = txtSubjectCode.Text;
            string subjectName = txtSubjectName.Text;
            int semesterId = int.Parse(ddlSemester.SelectedValue);
            int departmentId = int.Parse(ddlDepartment.SelectedValue);

            // Validate length constraints
            if (subjectCode.Length > 20) subjectCode = subjectCode.Substring(0, 20);
            if (subjectName.Length > 100) subjectName = subjectName.Substring(0, 100);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Subjects (subject_code, subject_name, semester_id, department_id) VALUES (@subject_code, @subject_name, @semester_id, @department_id)", conn);
                cmd.Parameters.AddWithValue("@subject_code", subjectCode);
                cmd.Parameters.AddWithValue("@subject_name", subjectName);
                cmd.Parameters.AddWithValue("@semester_id", semesterId);
                cmd.Parameters.AddWithValue("@department_id", departmentId);
                cmd.ExecuteNonQuery();
            }

            LoadSubjects();
            CloseModal("subjectModal");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('Subject added successfully');", true);
        }

        // Common methods
        private void CloseModal(string modalId)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", $"closeModal('{modalId}');", true);
        }

        protected void gvSemesters_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSemesters.EditIndex = e.NewEditIndex;
            LoadSemesters();
        }

        protected void gvSemesters_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSemesters.EditIndex = -1;
            LoadSemesters();
        }

        protected void gvSemesters_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvSemesters.Rows[e.RowIndex];
            int id = Convert.ToInt32(gvSemesters.DataKeys[e.RowIndex].Value);
            string semesterName = (row.Cells[1].Controls[0] as TextBox).Text.Trim();
            string yearName = (row.Cells[2].Controls[0] as TextBox).Text.Trim();

            // Validate length constraints
            if (semesterName.Length > 20) semesterName = semesterName.Substring(0, 20);
            if (yearName.Length > 30) yearName = yearName.Substring(0, 30);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Semesters SET semester_name=@semester_name, year_name=@year_name WHERE semester_id=@semester_id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@semester_name", semesterName);
                cmd.Parameters.AddWithValue("@year_name", yearName);
                cmd.Parameters.AddWithValue("@semester_id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            gvSemesters.EditIndex = -1;
            LoadSemesters();
        }

        protected void gvSemesters_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvSemesters.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "DELETE FROM Semesters WHERE semester_id=@semester_id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@semester_id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadSemesters();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('Semester deleted successfully');", true);
        }

        protected void gvDepartments_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvDepartments.EditIndex = e.NewEditIndex;
            LoadDepartments();
        }

        protected void gvDepartments_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDepartments.EditIndex = -1;
            LoadDepartments();
        }

        protected void gvDepartments_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvDepartments.Rows[e.RowIndex];
            int id = Convert.ToInt32(gvDepartments.DataKeys[e.RowIndex].Value);
            string departmentName = (row.Cells[1].Controls[0] as TextBox).Text;
            string room = (row.Cells[2].Controls[0] as TextBox).Text;

            // Validate length constraints
            if (departmentName.Length > 100) departmentName = departmentName.Substring(0, 100);
            if (room.Length > 10) room = room.Substring(0, 10);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Departments SET departmentname=@departmentname, room=@room WHERE departmentId=@departmentId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@departmentname", departmentName);
                cmd.Parameters.AddWithValue("@room", room);
                cmd.Parameters.AddWithValue("@departmentId", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            gvDepartments.EditIndex = -1;
            LoadDepartments();
        }

        protected void gvDepartments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvDepartments.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "DELETE FROM Departments WHERE departmentId=@departmentId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@departmentId", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadDepartments();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('Department deleted successfully');", true);
        }

        protected void gvSubjects_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSubjects.EditIndex = e.NewEditIndex;
            LoadSubjects();
        }

        protected void gvSubjects_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSubjects.EditIndex = -1;
            LoadSubjects();
        }

        protected void gvSubjects_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvSubjects.Rows[e.RowIndex];
            int id = Convert.ToInt32(gvSubjects.DataKeys[e.RowIndex].Value);
            string subjectCode = (row.Cells[1].Controls[0] as TextBox).Text;
            string subjectName = (row.Cells[2].Controls[0] as TextBox).Text;

            DropDownList ddlSemesterId = (DropDownList)row.FindControl("ddlSemesterId");
            int semesterId = Convert.ToInt32(ddlSemesterId.SelectedValue);

            DropDownList ddlDepartmentId = (DropDownList)row.FindControl("ddlDepartmentId");
            int departmentId = Convert.ToInt32(ddlDepartmentId.SelectedValue);

            // Validate length constraints
            if (subjectCode.Length > 20) subjectCode = subjectCode.Substring(0, 20);
            if (subjectName.Length > 100) subjectName = subjectName.Substring(0, 100);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Subjects SET subject_code=@subject_code, subject_name=@subject_name, semester_id=@semester_id, department_id=@department_id WHERE subject_id=@subject_id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@subject_code", subjectCode);
                cmd.Parameters.AddWithValue("@subject_name", subjectName);
                cmd.Parameters.AddWithValue("@semester_id", semesterId);
                cmd.Parameters.AddWithValue("@department_id", departmentId);
                cmd.Parameters.AddWithValue("@subject_id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            gvSubjects.EditIndex = -1;
            LoadSubjects();
        }

        protected void gvSubjects_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvSubjects.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "DELETE FROM Subjects WHERE subject_id=@subject_id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@subject_id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadSubjects();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('Subject deleted successfully');", true);
        }

        protected void gvSemesters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                // No additional dropdowns to bind for semesters
            }
        }

        protected void gvSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlSemesterId = (DropDownList)e.Row.FindControl("ddlSemesterId");
                DropDownList ddlDepartmentId = (DropDownList)e.Row.FindControl("ddlDepartmentId");

                if (ddlSemesterId != null)
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT semester_id, semester_name FROM Semesters", conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        ddlSemesterId.DataSource = dt;
                        ddlSemesterId.DataTextField = "semester_name";
                        ddlSemesterId.DataValueField = "semester_id";
                        ddlSemesterId.DataBind();

                        string semesterId = DataBinder.Eval(e.Row.DataItem, "semester_id").ToString();
                        ddlSemesterId.SelectedValue = semesterId;
                    }
                }

                if (ddlDepartmentId != null)
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT departmentId, departmentname FROM Departments", conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        ddlDepartmentId.DataSource = dt;
                        ddlDepartmentId.DataTextField = "departmentname";
                        ddlDepartmentId.DataValueField = "departmentId";
                        ddlDepartmentId.DataBind();

                        string departmentId = DataBinder.Eval(e.Row.DataItem, "department_id").ToString();
                        ddlDepartmentId.SelectedValue = departmentId;
                    }
                }
            }
        }
    }
}