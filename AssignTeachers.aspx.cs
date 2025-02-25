using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS
{
    public partial class AssignTeachers : System.Web.UI.Page
    {
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAssignments();
                LoadTeachers();
                LoadDepartments();
                LoadSubjects();
                LoadSemesters();
            }
        }

        private void LoadAssignments()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT ts.id, t.FullName AS teacher_name, d.departmentname AS department_name, s.subject_name, sem.semester_name, ts.time, ts.date FROM TeacherSubjects ts JOIN Teachers t ON ts.teacher_id = t.TeacherID JOIN Subjects s ON ts.subject_id = s.subject_id JOIN Semesters sem ON ts.semester_id = sem.semester_id JOIN Departments d ON t.department_id = d.departmentId", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvAssignments.DataSource = dt;
                gvAssignments.DataBind();
            }
        }

        private void LoadTeachers()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT TeacherID, FullName FROM Teachers", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlTeachers.DataSource = dt;
                ddlTeachers.DataTextField = "FullName";
                ddlTeachers.DataValueField = "TeacherID";
                ddlTeachers.DataBind();
            }
        }

        private void LoadDepartments()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT departmentId, departmentname FROM Departments", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlDepartments.DataSource = dt;
                ddlDepartments.DataTextField = "departmentname";
                ddlDepartments.DataValueField = "departmentId";
                ddlDepartments.DataBind();
            }
        }

        private void LoadSubjects()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT subject_id, subject_name FROM Subjects", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlSubjects.DataSource = dt;
                ddlSubjects.DataTextField = "subject_name";
                ddlSubjects.DataValueField = "subject_id";
                ddlSubjects.DataBind();
            }
        }

        private void LoadSemesters()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT semester_id, semester_name FROM Semesters", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlSemesters.DataSource = dt;
                ddlSemesters.DataTextField = "semester_name";
                ddlSemesters.DataValueField = "semester_id";
                ddlSemesters.DataBind();
            }
        }

        protected void btnShowModal_Click(object sender, EventArgs e)
        {
            pnlModal.Style["display"] = "block";
        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            int teacherId = int.Parse(ddlTeachers.SelectedValue);
            int subjectId = int.Parse(ddlSubjects.SelectedValue);
            int semesterId = int.Parse(ddlSemesters.SelectedValue);
            string time = ddlTime.SelectedValue;
            string date = ddlDate.SelectedValue;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO TeacherSubjects (teacher_id, subject_id, semester_id, time, date) VALUES (@teacher_id, @subject_id, @semester_id, @time, @date)", conn);
                cmd.Parameters.AddWithValue("@teacher_id", teacherId);
                cmd.Parameters.AddWithValue("@subject_id", subjectId);
                cmd.Parameters.AddWithValue("@semester_id", semesterId);
                cmd.Parameters.AddWithValue("@time", time);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.ExecuteNonQuery();
            }

            LoadAssignments();
            pnlModal.Style["display"] = "none";
        }

        protected void gvAssignments_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvAssignments.EditIndex = e.NewEditIndex;
            LoadAssignments();
        }

        protected void gvAssignments_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvAssignments.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvAssignments.Rows[e.RowIndex];
            string teacherName = (row.FindControl("txtTeacherName") as TextBox).Text;
            string departmentName = (row.FindControl("ddlDepartments") as DropDownList).SelectedItem.Text;
            string subjectName = (row.FindControl("ddlSubjects") as DropDownList).SelectedItem.Text;
            int semesterId = int.Parse((row.FindControl("ddlSemesters") as DropDownList).SelectedValue);
            string time = (row.FindControl("ddlTime") as DropDownList).SelectedItem.Text;
            string date = (row.FindControl("ddlDate") as DropDownList).SelectedItem.Text;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE TeacherSubjects SET teacher_id = (SELECT TeacherID FROM Teachers WHERE FullName = @teacher_name), subject_id = (SELECT subject_id FROM Subjects WHERE subject_name = @subject_name), semester_id = @semester_id, time = @time, date = @date WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@teacher_name", teacherName);
                cmd.Parameters.AddWithValue("@subject_name", subjectName);
                cmd.Parameters.AddWithValue("@semester_id", semesterId);
                cmd.Parameters.AddWithValue("@time", time);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            gvAssignments.EditIndex = -1;
            LoadAssignments();
        }

        protected void gvAssignments_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAssignments.EditIndex = -1;
            LoadAssignments();
        }

        protected void gvAssignments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvAssignments.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM TeacherSubjects WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            LoadAssignments();
        }

        protected void gvAssignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlDepartments = (DropDownList)e.Row.FindControl("ddlDepartments");
                DropDownList ddlSubjects = (DropDownList)e.Row.FindControl("ddlSubjects");
                DropDownList ddlSemesters = (DropDownList)e.Row.FindControl("ddlSemesters");
                DropDownList ddlTime = (DropDownList)e.Row.FindControl("ddlTime");
                DropDownList ddlDate = (DropDownList)e.Row.FindControl("ddlDate");

                if (ddlDepartments != null)
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT departmentId, departmentname FROM Departments", conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        ddlDepartments.DataSource = dt;
                        ddlDepartments.DataTextField = "departmentname";
                        ddlDepartments.DataValueField = "departmentId";
                        ddlDepartments.DataBind();

                        // Set the selected value
                        string departmentName = ((DataRowView)e.Row.DataItem)["department_name"].ToString();
                        ListItem item = ddlDepartments.Items.FindByText(departmentName);
                        if (item != null)
                        {
                            item.Selected = true;
                        }
                    }
                }

                if (ddlSubjects != null)
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT subject_id, subject_name FROM Subjects", conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        ddlSubjects.DataSource = dt;
                        ddlSubjects.DataTextField = "subject_name";
                        ddlSubjects.DataValueField = "subject_id";
                        ddlSubjects.DataBind();

                        // Set the selected value
                        string subjectName = ((DataRowView)e.Row.DataItem)["subject_name"].ToString();
                        ListItem item = ddlSubjects.Items.FindByText(subjectName);
                        if (item != null)
                        {
                            item.Selected = true;
                        }
                    }
                }

                if (ddlSemesters != null)
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        SqlDataAdapter da = new SqlDataAdapter("SELECT semester_id, semester_name FROM Semesters", conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        ddlSemesters.DataSource = dt;
                        ddlSemesters.DataTextField = "semester_name";
                        ddlSemesters.DataValueField = "semester_id";
                        ddlSemesters.DataBind();

                        // Set the selected value
                        string semesterName = ((DataRowView)e.Row.DataItem)["semester_name"].ToString();
                        ListItem item = ddlSemesters.Items.FindByText(semesterName);
                        if (item != null)
                        {
                            item.Selected = true;
                        }
                    }
                }

                if (ddlTime != null)
                {
                    // Set the selected value
                    string time = ((DataRowView)e.Row.DataItem)["time"].ToString();
                    ListItem item = ddlTime.Items.FindByText(time);
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }

                if (ddlDate != null)
                {
                    // Set the selected value
                    string date = ((DataRowView)e.Row.DataItem)["date"].ToString();
                    ListItem item = ddlDate.Items.FindByText(date);
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
            }
        }
    }
}