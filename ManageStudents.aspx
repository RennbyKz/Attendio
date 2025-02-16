<%@ Page Title="Manage Students" Language="C#" MasterPageFile="~/admindashboard.Master" AutoEventWireup="true" CodeBehind="ManageStudents.aspx.cs" Inherits="AMS.ManageStudents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Students</h2>
    
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>


    <!-- Add New Student Form -->   
    <asp:Button ID="btnAddStudent" runat="server" CssClass="btn btn-success mb-4" Text="Add New Student" OnClientClick="openModal(); return false;" /> <br /><br />

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
        OnRowCancelingEdit="gvStudents_RowCancelingEdit" OnRowUpdating="gvStudents_RowUpdating" OnRowDeleting="gvStudents_RowDeleting">
        <Columns>
            <asp:BoundField DataField="id" HeaderText="ID" ReadOnly="True" Visible="false" />
            <asp:BoundField DataField="rollno" HeaderText="Rno." />
            <asp:BoundField DataField="formatted_rollno" HeaderText="RollNo." ReadOnly="True" />
            <asp:BoundField DataField="name" HeaderText="Full Name" />
            <asp:BoundField DataField="email" HeaderText="Email" /> 
            <asp:BoundField DataField="semester_name" HeaderText="Semester" ReadOnly="True" />
            <asp:BoundField DataField="year_name" HeaderText="Year" ReadOnly="True" />

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

           <asp:TemplateField HeaderText="Major">
            <ItemTemplate>
                <asp:Label ID="lblMajor" runat="server" Text='<%# Eval("major") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlMajor" runat="server">
                    <asp:ListItem Text="CST" Value="CST"></asp:ListItem>
                    <asp:ListItem Text="CS" Value="CS"></asp:ListItem>
                    <asp:ListItem Text="CT" Value="CT"></asp:ListItem>
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

           <asp:TemplateField HeaderText="Sem_ID">
            <ItemTemplate>
                <asp:Label ID="lblSemesterId" runat="server" Text='<%# Eval("semester_id") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlSemesterId" runat="server">
                    <asp:ListItem Text="Semester-1" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Semester-2" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Semester-3" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Semester-4" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Semester-5" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Semester-6" Value="6"></asp:ListItem>
                    <asp:ListItem Text="Semester-7" Value="7"></asp:ListItem>
                    <asp:ListItem Text="Semester-8" Value="8"></asp:ListItem>
                    <asp:ListItem Text="Semester-9" Value="9"></asp:ListItem>
                    <asp:ListItem Text="Semester-10" Value="10"></asp:ListItem>
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

    <!-- Modal for Add New Student -->
    <div id="modalForm" class="modal">
        <div class="modal-content">
            <span class="close" onclick="closeModal();">&times;</span>
            <h3 id="modalTitleAdd">Add New Student</h3>
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
                        <asp:ListItem Text="CST" Value="CST"></asp:ListItem>
                        <asp:ListItem Text="CS" Value="CS"></asp:ListItem>
                        <asp:ListItem Text="CT" Value="CT"></asp:ListItem>
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
                         <asp:ListItem Text="Semester-1" Value="1"></asp:ListItem>
                         <asp:ListItem Text="Semester-2" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Semester-3" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Semester-4" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Semester-5" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Semester-6" Value="6"></asp:ListItem>
                            <asp:ListItem Text="Semester-7" Value="7"></asp:ListItem>
                            <asp:ListItem Text="Semester-8" Value="8"></asp:ListItem>
                            <asp:ListItem Text="Semester-9" Value="9"></asp:ListItem>
                            <asp:ListItem Text="Semester-10" Value="10"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-4 mt-4">
                    <asp:Button ID="btnSaveStudent" runat="server" CssClass="btn btn-primary" Text="Save Student" OnClick="btnSaveStudent_Click" />
                </div>
            </div>
        </div>
    </div>
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css">

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
