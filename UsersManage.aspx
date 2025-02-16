<%@ Page Title="Manage Users" Language="C#" MasterPageFile="~/admindashboard.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="AMS.ManageUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Users</h2>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

    <!-- Add New User Button -->   
    <asp:Button ID="btnAddUser" runat="server" CssClass="btn btn-success mb-4" Text="Add New User" OnClientClick="openModal(); return false;" /><br /><br>

    <!-- Search and Filter Controls -->
    <div class="search-filter-controls">
        <asp:Label runat="server" Text="Search by Username: "></asp:Label>
        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn-search" />

        <asp:Label runat="server" Text="Filter by Role: "></asp:Label>
        <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
            <asp:ListItem Text="All" Value="All"></asp:ListItem>
            <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
            <asp:ListItem Text="Student" Value="Student"></asp:ListItem>
            <asp:ListItem Text="Teacher" Value="Teacher"></asp:ListItem>
        </asp:DropDownList>
            <div class="total-users">
                <i class="fa fa-users"></i>
                <span>Total users:</span> 
                <asp:Label ID="lblTotalUsers" runat="server"></asp:Label>
            </div>
    </div>
    <!-- Label to Display Total Users -->
    

    <!-- Users GridView -->
    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="id" OnRowEditing="gvUsers_RowEditing"
        OnRowCancelingEdit="gvUsers_RowCancelingEdit" OnRowUpdating="gvUsers_RowUpdating" OnRowDeleting="gvUsers_RowDeleting">
        <Columns>
            <asp:BoundField DataField="id" HeaderText="ID" ReadOnly="True" Visible="False" />
            <asp:BoundField DataField="username" HeaderText="Username" ReadOnly="true" />
            <asp:BoundField DataField="role" HeaderText="Role" />
            <asp:BoundField DataField="RelatedName" HeaderText="Related Name" ReadOnly="true" />
            <asp:BoundField DataField="created_at" HeaderText="Created At" ReadOnly="true" />
            <asp:TemplateField HeaderText="Actions">
                <EditItemTemplate>
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CommandName="Update" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CommandName="Cancel" />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView><br /> 

    <!-- Modal for Add admin User -->
<div id="modalForm" class="modal">
    <div class="modal-content">
        <span class="close" onclick="closeModal();">&times;</span>
        <h3 id="modalTitle">Add New User</h3>
        <div class="row">
            <div class="col-md-4">
                <label>Username:</label>
                <asp:TextBox ID="txtUsernameModal" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-4">
                <label>Password:</label>
                <asp:TextBox ID="txtPasswordModal" runat="server" TextMode="Password" CssClass="form-control" />
            </div>
            <div class="col-md-4">
                <label>Name:</label>
                <asp:TextBox ID="textnameModal" runat="server" CssClass="form-control" /> <!-- Ensure ID matches -->
            </div>
            <div class="col-md-4">
                <label>Role:</label>
                <asp:DropDownList ID="ddlRoleModal" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                    <asp:ListItem Text="Student" Value="Student"></asp:ListItem>
                    <asp:ListItem Text="Teacher" Value="Teacher"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-4 mt-4">
                <asp:Button ID="btnSaveUser" runat="server" CssClass="btn btn-primary" Text="Save User" OnClick="btnSaveUser_Click" />
            </div>
        </div>
    </div>
</div>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css">


    <!-- Generate Users Buttons -->
    <asp:Button ID="btnGenerateStudents" runat="server" Text="Generate Students" OnClick="btnGenerateStudents_Click" CssClass="btn btn-primary" />
     &nbsp;
    <asp:Button ID="btnGenerateTeachers" runat="server" Text="Generate Teachers" OnClick="btnGenerateTeachers_Click" CssClass="btn btn-primary" />

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
