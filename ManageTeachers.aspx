<%@ Page Title="Manage Teachers" Language="C#" MasterPageFile="~/admindashboard.Master" AutoEventWireup="true" CodeBehind="ManageTeachers.aspx.cs" Inherits="AMS.ManageTeachers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Teachers</h2>
    
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

    <!-- Add New Teacher Button -->   
    <asp:Button ID="btnAddTeacher" runat="server" CssClass="btn btn-success mb-4" Text="Add New Teacher" OnClientClick="openModal(); return false;" /><br /><br>

    <!-- Search and Filter Controls -->
    <div class="search-filter-controls">
        <asp:Label runat="server" Text="Search by Name: "></asp:Label>
        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn-search" />
    
        <asp:Label runat="server" Text="Filter by Department: "></asp:Label>
        <asp:DropDownList ID="ddlDepartment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
        </asp:DropDownList>
        <div class="total-users">
            <i class="fa fa-user-graduate"></i>
            <span>Total Teachers:</span> 
            <asp:Label ID="lblTotalUsers" runat="server"></asp:Label>
        </div>
    </div>

    <!-- Teacher GridView -->
    <asp:GridView ID="gvTeachers" runat="server" AutoGenerateColumns="False" DataKeyNames="TeacherID" OnRowEditing="gvTeachers_RowEditing"
        OnRowCancelingEdit="gvTeachers_RowCancelingEdit" OnRowUpdating="gvTeachers_RowUpdating" OnRowDeleting="gvTeachers_RowDeleting" OnRowDataBound="gvTeachers_RowDataBound">
        <Columns>
            <asp:BoundField DataField="TeacherID" HeaderText="ID" ReadOnly="True" Visible="False" />
            <asp:BoundField DataField="FullName" HeaderText="Full Name" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
            <asp:BoundField DataField="DepartmentName" HeaderText="Department Name" ReadOnly="true" />
            <asp:TemplateField HeaderText="Department ID">
                <ItemTemplate>
                    <asp:Label ID="lblDepartmentId" runat="server" Text='<%# Eval("departmentid") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlDepartmentId" runat="server">
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
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Role">
                <ItemTemplate>
                    <asp:Label ID="lblRole" runat="server" Text='<%# Eval("Role") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlRole" runat="server">
                        <asp:ListItem Text="Rector" Value="Rector"></asp:ListItem>
                        <asp:ListItem Text="Vice-Rector" Value="Vice-Rector"></asp:ListItem>
                        <asp:ListItem Text="Professor" Value="Professor"></asp:ListItem>
                        <asp:ListItem Text="Associate Professor" Value="Associate Professor"></asp:ListItem>
                        <asp:ListItem Text="Lecturer" Value="Lecturer"></asp:ListItem>
                        <asp:ListItem Text="Assistant Lecturer" Value="Assistant Lecturer"></asp:ListItem>
                        <asp:ListItem Text="Tutor" Value="Tutor"></asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                        <asp:ListItem Text="Resigned" Value="Resigned"></asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>

    <!-- Modal for Add Teacher -->
    <!-- Modal for Add Teacher -->
<div id="modalForm" class="modal">
    <div class="modal-content">
        <span class="close" onclick="closeModal();">&times;</span>
        <h3 id="modalTitle">Add New Teacher</h3>
        <div class="row">
            <div class="col-md-4">
                <label>Full Name:</label>
                <asp:TextBox ID="txtFullNameModal" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-4">
                <label>Email:</label>
                <asp:TextBox ID="txtEmailModal" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-4">
                <label>Phone Number:</label>
                <asp:TextBox ID="txtPhoneModal" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-4">
                <label>Department:</label>
                <asp:DropDownList ID="ddlDepartmentModal" runat="server" CssClass="form-control">
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
            <div class="col-md-4">
                <label>Role:</label>
                <asp:DropDownList ID="ddlRoleModal" runat="server" CssClass="form-control">
                  <asp:ListItem Text="Rector" Value="Rector"></asp:ListItem>
                <asp:ListItem Text="Vice-Rector" Value="Vice-Rector"></asp:ListItem>
                <asp:ListItem Text="Professor" Value="Professor"></asp:ListItem>
                <asp:ListItem Text="Associate Professor" Value="Associate Professor"></asp:ListItem>
                <asp:ListItem Text="Lecturer" Value="Lecturer"></asp:ListItem>
                <asp:ListItem Text="Assistant Lecturer" Value="Assistant Lecturer"></asp:ListItem>
                <asp:ListItem Text="Tutor" Value="Tutor"></asp:ListItem>
            </asp:DropDownList>

            </div>
            <div class="col-md-4 mt-4">
                <asp:Button ID="btnSaveTeacher" runat="server" CssClass="btn btn-primary" Text="Save Teacher" OnClick="btnSaveTeacher_Click" />
            </div>
        </div>
    </div>
</div><br><br><br>
    <footer>@Copyright 2025 Student Affair</footer>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css">

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <link rel="stylesheet" href="css/ManageStudents.css">
    <script>
        function openModal() {
            document.getElementById('modalForm').style.display = 'block';
        }
        function closeModal() {
            document.getElementById('modalForm').style.display = 'none';
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