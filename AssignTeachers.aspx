<%@ Page Title="" Language="C#" MasterPageFile="~/admindashboard.Master" AutoEventWireup="true" CodeBehind="AssignTeachers.aspx.cs" Inherits="AMS.AssignTeachers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Assign Teachers to Subjects</h2>

    <asp:Button ID="btnShowModal" runat="server" Text="Add New Assignment" OnClick="btnShowModal_Click" style=" background-color: #48bed9; /* color for the search button */
             color: #fff;
             border: none;
             padding: 10px 20px;
             border-radius: 100px;
             cursor: pointer;
             font-size: 16px;" />

    <asp:GridView ID="gvAssignments" runat="server" AutoGenerateColumns="False" DataKeyNames="id" OnRowEditing="gvAssignments_RowEditing" OnRowUpdating="gvAssignments_RowUpdating" OnRowCancelingEdit="gvAssignments_RowCancelingEdit" OnRowDeleting="gvAssignments_RowDeleting" OnRowDataBound="gvAssignments_RowDataBound">
        <Columns>
            <asp:BoundField DataField="id" HeaderText="ID" ReadOnly="True" InsertVisible="False" />
            <asp:TemplateField HeaderText="Teacher Name">
                <ItemTemplate>
                    <asp:Label ID="lblTeacherName" runat="server" Text='<%# Eval("teacher_name") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtTeacherName" runat="server" Text='<%# Eval("teacher_name") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Department Name">
                <ItemTemplate>
                    <asp:Label ID="lblDepartment" runat="server" Text='<%# Eval("department_name") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlDepartments" runat="server" />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Subject Name">
                <ItemTemplate>
                    <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("subject_name") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlSubjects" runat="server" />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Semester Name">
                <ItemTemplate>
                    <asp:Label ID="lblSemester" runat="server" Text='<%# Eval("semester_name") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlSemesters" runat="server" />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Time">
                <ItemTemplate>
                    <asp:Label ID="lblTime" runat="server" Text='<%# Eval("time") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlTime" runat="server">
                        <asp:ListItem Value="9:00-11:00">9:00-11:00</asp:ListItem>
                        <asp:ListItem Value="12:00-2:00">12:00-2:00</asp:ListItem>
                        <asp:ListItem Value="2:00-4:00">2:00-4:00</asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Date">
                <ItemTemplate>
                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("date") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlDate" runat="server">
                        <asp:ListItem Value="Monday">Monday</asp:ListItem>
                        <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                        <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                        <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                        <asp:ListItem Value="Friday">Friday</asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
        </Columns>

    </asp:GridView>
     <footer>@Copyright 2025 Student Affair</footer>
    <asp:Panel ID="pnlModal" runat="server" CssClass="modal" Style="display:none;">
        <div class="modal-content">
            <span class="close" onclick="document.getElementById('<%= pnlModal.ClientID %>').style.display='none'">&times;</span>
            <h3>Add New Assignment</h3>
            <div>
                <asp:Label ID="lblTeacher" runat="server" Text="Teacher: " />
                <asp:DropDownList ID="ddlTeachers" runat="server" />
            </div>
            <div>
                <asp:Label ID="lblDepartment" runat="server" Text="Department: " />
                <asp:DropDownList ID="ddlDepartments" runat="server" />
            </div>
            <div>
                <asp:Label ID="lblSubject" runat="server" Text="Subject: " />
                <asp:DropDownList ID="ddlSubjects" runat="server" />
            </div>
            <div>
                <asp:Label ID="lblSemester" runat="server" Text="Semester: " />
                <asp:DropDownList ID="ddlSemesters" runat="server" />
            </div>
            <div>
                <asp:Label ID="lblTime" runat="server" Text="Time: " />
                <asp:DropDownList ID="ddlTime" runat="server">
                    <asp:ListItem Value="9:00-11:00">9:00-11:00</asp:ListItem>
                    <asp:ListItem Value="12:00-2:00">12:00-2:00</asp:ListItem>
                    <asp:ListItem Value="2:00-4:00">2:00-4:00</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div>
                <asp:Label ID="lblDate" runat="server" Text="Date: " />
                <asp:DropDownList ID="ddlDate" runat="server">
                    <asp:ListItem Value="Monday">Monday</asp:ListItem>
                    <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                    <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                    <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                    <asp:ListItem Value="Friday">Friday</asp:ListItem>
                </asp:DropDownList>
            </div><br />
            <div>
                <asp:Button ID="btnAssign" runat="server" Text="Assign" OnClick="btnAssign_Click" style=" background-color: #48bed9; /* color for the search button */
                     color: #fff;
                     border: none;
                     padding: 10px 20px;
                     border-radius: 100px;
                     cursor: pointer;
                     font-size: 16px;"/>
            </div>
            
        </div>
          
        <style>

            footer {
    background: linear-gradient(to bottom, #00e2e0, #aae5f2); /* University theme color */
    color: #fff;
    text-align: center;
    padding: 10px 0;
    position: fixed;
    bottom: 0;
    width: 100%;
    font-size: 14px;
}
        </style>
        <link rel="stylesheet" href="css/ManageStudents.css">
     
    </asp:Panel>
</asp:Content>