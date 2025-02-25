<%@ Page Title="Manage Entities" Language="C#" MasterPageFile="~/admindashboard.Master" AutoEventWireup="true" CodeBehind="ManageEntities.aspx.cs" Inherits="AMS.ManageEntities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Entities</h2>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

    <!-- Table Selection Dropdown -->
    <asp:Label runat="server" Text="Select Table: "></asp:Label>
    <asp:DropDownList ID="ddlTableSelection" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTableSelection_SelectedIndexChanged" style="width: 150px;">
        <asp:ListItem Text="Semesters" Value="Semesters"></asp:ListItem>
        <asp:ListItem Text="Departments" Value="Departments"></asp:ListItem>
        <asp:ListItem Text="Subjects" Value="Subjects"></asp:ListItem>
    </asp:DropDownList><br />

    <!-- Semesters Section -->
    <div id="semestersSection" runat="server" visible="false">
        <h3>Manage Semesters</h3>
        <asp:Button ID="btnAddSemester" runat="server" CssClass="btn btn-success mb-4" Text="Add New Semester" OnClientClick="openModal('semesterModal'); return false;" /><br /><br>
        <asp:GridView ID="gvSemesters" runat="server" AutoGenerateColumns="False" DataKeyNames="semester_id" OnRowEditing="gvSemesters_RowEditing"
            OnRowCancelingEdit="gvSemesters_RowCancelingEdit" OnRowUpdating="gvSemesters_RowUpdating" OnRowDeleting="gvSemesters_RowDeleting" OnRowDataBound="gvSemesters_RowDataBound">
            <Columns>
                <asp:BoundField DataField="semester_id" HeaderText="ID" ReadOnly="True" Visible="False" />
                <asp:BoundField DataField="semester_name" HeaderText="Semester Name" />
                <asp:BoundField DataField="year_name" HeaderText="Year Name" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- Departments Section -->
    <div id="departmentsSection" runat="server" visible="false">
        <h3>Manage Departments</h3>
        <asp:Button ID="btnAddDepartment" runat="server" CssClass="btn btn-success mb-4" Text="Add New Department" OnClientClick="openModal('departmentModal'); return false;" /><br /><br>
        <asp:GridView ID="gvDepartments" runat="server" AutoGenerateColumns="False" DataKeyNames="departmentId" OnRowEditing="gvDepartments_RowEditing"
            OnRowCancelingEdit="gvDepartments_RowCancelingEdit" OnRowUpdating="gvDepartments_RowUpdating" OnRowDeleting="gvDepartments_RowDeleting">
            <Columns>
                <asp:BoundField DataField="departmentId" HeaderText="ID" ReadOnly="True" Visible="False" />
                <asp:BoundField DataField="departmentname" HeaderText="Department Name" />
                <asp:BoundField DataField="room" HeaderText="Room" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- Subjects Section -->
    <div id="subjectsSection" runat="server" visible="false">
        <h3>Manage Subjects</h3>
        <asp:Button ID="btnAddSubject" runat="server" CssClass="btn btn-success mb-4" Text="Add New Subject" OnClientClick="openModal('subjectModal'); return false;" /><br /><br>
        <asp:GridView ID="gvSubjects" runat="server" AutoGenerateColumns="False" DataKeyNames="subject_id" OnRowEditing="gvSubjects_RowEditing"
            OnRowCancelingEdit="gvSubjects_RowCancelingEdit" OnRowUpdating="gvSubjects_RowUpdating" OnRowDeleting="gvSubjects_RowDeleting" OnRowDataBound="gvSubjects_RowDataBound">
            <Columns>
                <asp:BoundField DataField="subject_id" HeaderText="ID" ReadOnly="True" Visible="False" />
                <asp:BoundField DataField="subject_code" HeaderText="Subject Code" />
                <asp:BoundField DataField="subject_name" HeaderText="Subject Name" />
                <asp:TemplateField HeaderText="Semester">
                    <ItemTemplate>
                        <asp:Label ID="lblSemesterId" runat="server" Text='<%# Eval("semester_id") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlSemesterId" runat="server">
                           
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Department">
                    <ItemTemplate>
                        <asp:Label ID="lblDepartmentId" runat="server" Text='<%# Eval("department_id") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlDepartmentId" runat="server">
                           
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- Modal for Add Semester -->
    <div id="semesterModal" class="modal">
        <div class="modal-content">
            <span class="close" onclick="closeModal('semesterModal');">&times;</span>
            <h3>Add New Semester</h3>
            <div class="row">
                <div class="col-md-4">
                    <label>Semester Name:</label>
                    <asp:TextBox ID="txtSemesterName" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <label>Year Name:</label>
                    <asp:TextBox ID="txtYearName" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4 mt-4">
                    <asp:Button ID="btnSaveSemester" runat="server" CssClass="btn btn-primary" Text="Save Semester" OnClick="btnSaveSemester_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal for Add Department -->
    <div id="departmentModal" class="modal">
        <div class="modal-content">
            <span class="close" onclick="closeModal('departmentModal');">&times;</span>
            <h3>Add New Department</h3>
            <div class="row">
                <div class="col-md-4">
                    <label>Department Name:</label>
                    <asp:TextBox ID="txtDepartmentName" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <label>Room:</label>
                    <asp:TextBox ID="txtDepartmentRoom" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4 mt-4">
                    <asp:Button ID="btnSaveDepartment" runat="server" CssClass="btn btn-primary" Text="Save Department" OnClick="btnSaveDepartment_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal for Add Subject -->
    <div id="subjectModal" class="modal">
        <div class="modal-content">
            <span class="close" onclick="closeModal('subjectModal');">&times;</span>
            <h3>Add New Subject</h3>
            <div class="row">
                <div class="col-md-4">
                    <label>Subject Code:</label>
                    <asp:TextBox ID="txtSubjectCode" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <label>Subject Name:</label>
                    <asp:TextBox ID="txtSubjectName" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <label>Semester:</label>
                    <asp:DropDownList ID="ddlSemester" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Semester-I" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Semester-II" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Semester-II" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Semester-IV" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Semester-V" Value="5"></asp:ListItem>
                        <asp:ListItem Text="Semester-VI" Value="6"></asp:ListItem>
                        <asp:ListItem Text="Semester-VII" Value="7"></asp:ListItem>
                        <asp:ListItem Text="Semester-VIII" Value="8"></asp:ListItem>
                        <asp:ListItem Text="Semester-IX" Value="9"></asp:ListItem>
                        <asp:ListItem Text="Semester-X" Value="10"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-4">
                    <label>Department:</label>
                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Information Science Department" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Department Of Myanmar" Value="2"></asp:ListItem>
                        <asp:ListItem Text="English Department" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Computational Mathematics Department" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Software Technology Department" Value="5"></asp:ListItem>
                        <asp:ListItem Text="Application Department" Value="6"></asp:ListItem>
                        <asp:ListItem Text="Hardware Technology Department" Value="8"></asp:ListItem>
                        <asp:ListItem Text="ITSM" Value="9"></asp:ListItem>
                        <asp:ListItem Text="Physic Department" Value="10"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-4 mt-4">
                    <asp:Button ID="btnSaveSubject" runat="server" CssClass="btn btn-primary" Text="Save Subject" OnClick="btnSaveSubject_Click" />
                </div>
            </div>
        </div>
    </div>
<br><br><br>
    <footer>@Copyright Student Affair 2025</footer>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css">

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <link rel="stylesheet" href="css/ManageStudents.css">
    <script>
        function openModal(modalId) {
            document.getElementById(modalId).style.display = 'block';
        }
        function closeModal(modalId) {
            document.getElementById(modalId).style.display = 'none';
        }
        function showSuccessMessage(message) {
            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: message,
                timer: 1500,
                showConfirmButton: false
            });
        }
        function showErrorMessage(message) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: message,
                timer: 1500,
                showConfirmButton: false
            });
        }
    </script>
</asp:Content>