using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace AMS
{
    public partial class ManageTeachers : System.Web.UI.Page
    {
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UCSPDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTeachers();
                LoadDepartments();
            }
        }

        private void LoadDepartments()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT departmentid, departmentname FROM Departments", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlDepartment.DataSource = dt;
                ddlDepartment.DataTextField = "departmentname";
                ddlDepartment.DataValueField = "departmentid";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem("All Departments", "0"));
            }
        }

        private void LoadTeachers(string searchQuery = "", int departmentId = 0)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"SELECT 
                            t.TeacherID, 
                            t.FullName, 
                            t.Email, 
                            t.PhoneNumber,
                            t.Status, 
                            dep.departmentid, 
                            dep.departmentname 
                         FROM 
                            Teachers t 
                         JOIN 
                            Departments dep ON t.department_id = dep.departmentId
                         WHERE 
                            (t.FullName LIKE @SearchQuery OR @SearchQuery = '') 
                            AND 
                            (t.department_id = @DepartmentId OR @DepartmentId = 0)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");
                cmd.Parameters.AddWithValue("@DepartmentId", departmentId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvTeachers.DataSource = dt;
                gvTeachers.DataBind();

                // Calculate and display total users
                lblTotalUsers.Text = dt.Rows.Count.ToString();
            }
        }

        protected void btnSaveTeacher_Click(object sender, EventArgs e)
        {
            string fullName = txtFullNameModal.Text;
            string email = txtEmailModal.Text;
            string phoneNumber = txtPhoneModal.Text;
            string department = ddlDepartmentModal.SelectedValue;



            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Teachers (FullName, Email, PhoneNumber, department_id) VALUES (@FullName, @Email, @PhoneNumber, @Department)", conn);
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("@Department", department);
         
                cmd.ExecuteNonQuery();
            }

            LoadTeachers();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('Teacher added successfully');", true);
        }


        private void CloseModal()
        {
            // Implementation to close the modal
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadTeachers(txtSearch.Text, int.Parse(ddlDepartment.SelectedValue));
        }
        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTeachers(txtSearch.Text, int.Parse(ddlDepartment.SelectedValue));
        }


        protected void gvTeachers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTeachers.EditIndex = e.NewEditIndex;
            LoadTeachers();
        }

        protected void gvTeachers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTeachers.EditIndex = -1;
            LoadTeachers();
        }

        protected void gvTeachers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvTeachers.Rows[e.RowIndex];
            int teacherID = Convert.ToInt32(gvTeachers.DataKeys[e.RowIndex].Value);
            string fullName = (row.Cells[1].Controls[0] as TextBox).Text;
            string email = (row.Cells[2].Controls[0] as TextBox).Text;
            string phoneNumber = (row.Cells[3].Controls[0] as TextBox).Text;

            DropDownList ddlDepartmentId = (DropDownList)row.FindControl("ddlDepartmentId");
            int departmentId = Convert.ToInt32(ddlDepartmentId.SelectedValue);


            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE Teachers SET FullName=@FullName, Email=@Email, PhoneNumber=@PhoneNumber, department_id=@Department WHERE TeacherID=@TeacherID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("@Department", departmentId);
               
                cmd.Parameters.AddWithValue("@TeacherID", teacherID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            gvTeachers.EditIndex = -1;
            LoadTeachers();
        }


        protected void gvStudents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {

                // Bind Department ID DropDownList
                DropDownList ddlDepartmentId = (DropDownList)e.Row.FindControl("ddlDepartmentId");
                string departmentId = DataBinder.Eval(e.Row.DataItem, "department_id").ToString();
                ddlDepartmentId.SelectedValue = departmentId;
            }
        }


        protected void gvTeachers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int teacherID = Convert.ToInt32(gvTeachers.DataKeys[e.RowIndex].Value);
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "DELETE FROM Teachers WHERE TeacherID=@TeacherID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TeacherID", teacherID);
                conn.Open();
                cmd.ExecuteNonQuery();

                // Reseed identity column so the next student gets the deleted ID.
                string reseedQuery = "DBCC CHECKIDENT ('Teacherss', RESEED, @TeacherId)";
                SqlCommand reseedCmd = new SqlCommand(reseedQuery, conn);
                reseedCmd.Parameters.AddWithValue("@TeacherId", teacherID);

                reseedCmd.ExecuteNonQuery();


            }
            LoadTeachers();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showSuccessMessage", "showSuccessMessage('Teacher deleted successfully');", true);
        }
    }
}
