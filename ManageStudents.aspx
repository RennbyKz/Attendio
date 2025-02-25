<%@ Page Title="Manage Students" Language="C#" MasterPageFile="~/admindashboard.Master" AutoEventWireup="true" CodeBehind="ManageStudents.aspx.cs" Inherits="AMS.ManageStudents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Students</h2>
    
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

    <!-- Add New Student Button -->   
    <asp:Button ID="btnAddStudent" runat="server" CssClass="btn btn-success mb-4" Text="Add New Student" OnClientClick="openModal(); return false;" /><br /><br>

    <!-- Search and Filter Controls -->
    <div class="search-filter-controls">
        <asp:Label runat="server" Text="Search by Name: "></asp:Label>
        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn-search" />
    
        <asp:Label runat="server" Text="Filter by Semester: "></asp:Label>
        <asp:DropDownList ID="ddlSemester" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSemester_SelectedIndexChanged">
        </asp:DropDownList>
        <div class="total-users">
            <i class="fa fa-user-graduate"></i>
            <span>Total Students:</span> 
            <asp:Label ID="lblTotalUsers" runat="server"></asp:Label>
        </div>
    </div>

    <!-- Student GridView -->
    <asp:GridView ID="gvStudents" runat="server" AutoGenerateColumns="False" DataKeyNames="id" OnRowEditing="gvStudents_RowEditing"
        OnRowCancelingEdit="gvStudents_RowCancelingEdit" OnRowUpdating="gvStudents_RowUpdating" OnRowDeleting="gvStudents_RowDeleting" OnRowDataBound="gvStudents_RowDataBound">
        <Columns>
            <asp:BoundField DataField="id" HeaderText="ID" ReadOnly="True" Visible="False" />
            <asp:BoundField DataField="rollno" HeaderText="Roll No" />
            <asp:BoundField DataField="formatted_rollno" HeaderText="Formatted Roll No" ReadOnly="True" />
            <asp:BoundField DataField="name" HeaderText="Name" />
            <asp:TemplateField HeaderText="Gender">
                <ItemTemplate>
                    <asp:Label ID="lblGender" runat="server" Text='<%# Eval("gender") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlGender" runat="server">
                        <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                        <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="email" HeaderText="Email" />
            <asp:TemplateField HeaderText="Major">
                <ItemTemplate>
                    <asp:Label ID="lblMajor" runat="server" Text='<%# Eval("major") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlMajor" runat="server">
                        <asp:ListItem Text="Computer Science" Value="CS"></asp:ListItem>
                        <asp:ListItem Text="Computer Technology" Value="CT"></asp:ListItem>
                        <asp:ListItem Text="Computer Science&Technology" Value="CST"></asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Section">
                <ItemTemplate>
                    <asp:Label ID="lblSection" runat="server" Text='<%# Eval("section") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlSection" runat="server">
                        <asp:ListItem Text="A" Value="A"></asp:ListItem>
                        <asp:ListItem Text="B" Value="B"></asp:ListItem>
                        <asp:ListItem Text="C" Value="C"></asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Semester">
                <ItemTemplate>
                    <asp:Label ID="lblSemesterId" runat="server" Text='<%# Eval("semester_id") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlSemesterId" runat="server">
                     
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                        <asp:ListItem Text="Graduated" Value="Graduated"></asp:ListItem>
                        <asp:ListItem Text="Left" Value="Left"></asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>

    <!-- Modal for Add Student -->
    <div id="modalForm" class="modal">
        <div class="modal-content">
            <span class="close" onclick="closeModal();">&times;</span>
            <h3 id="modalTitle">Add New Student</h3>
            <div class="row">
                <div class="col-md-4">
                    <label>Roll No:</label>
                    <asp:TextBox ID="txtRollNoModal" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <label>Name:</label>
                    <asp:TextBox ID="txtNameModal" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <label>Gender:</label>
                    <asp:DropDownList ID="ddlGenderModal" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                        <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-4">
                    <label>Email:</label>
                    <asp:TextBox ID="txtEmailModal" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <label>Major:</label>
                    <asp:DropDownList ID="ddlMajorModal" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Computer Science" Value="CS"></asp:ListItem>
                        <asp:ListItem Text="Computer Technology" Value="CT"></asp:ListItem>
                        <asp:ListItem Text="Computer Science&Technology" Value="CST"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-4">
                    <label>Section:</label>
                    <asp:DropDownList ID="ddlSectionModal" runat="server" CssClass="form-control">
                        <asp:ListItem Text="A" Value="A"></asp:ListItem>
                        <asp:ListItem Text="B" Value="B"></asp:ListItem>
                        <asp:ListItem Text="C" Value="C"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-4">
                    <label>Semester:</label>
                    <asp:DropDownList ID="ddlSemesterModal" runat="server" CssClass="form-control">
                      
                    </asp:DropDownList>
                </div>
                <div class="col-md-4 mt-4">
                    <asp:Button ID="btnSaveStudent" runat="server" CssClass="btn btn-primary" Text="Save Student" OnClick="btnSaveStudent_Click" />
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