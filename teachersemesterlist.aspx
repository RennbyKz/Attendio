<%@ Page Title="" Language="C#" MasterPageFile="~/teachers.Master" AutoEventWireup="true" CodeBehind="teachersemesterlist.aspx.cs" Inherits="AMS.teachersemesterlist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Teacher's Semester List</h2>
    
    <!-- jQuery -->
    <script src="js/jquery-3.6.0.min.js"></script>

    <!-- SweetAlert2 CSS and JS -->
    <link rel="stylesheet" href="css/sweetalert2.min.css">
    <script src="js/sweetalert2@11.js"></script>

    <!-- Bootstrap CSS and JS -->
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <script src="js/bootstrap.min.js"></script>

    <!-- Semester Buttons -->
    <asp:Repeater ID="rptSemesters" runat="server">
        <ItemTemplate>
            <asp:Button ID="btnSemester" runat="server" Text='<%# Eval("semester_name") %>' CommandArgument='<%# Eval("semester_id") %>' OnClick="btnSemester_Click" CssClass="btn btn-primary m-2" />
        </ItemTemplate>
    </asp:Repeater>
    
    <!-- Students Table -->
    <asp:Panel ID="studentsTable" runat="server" Visible="false">
        <h3>Student Attendance List</h3>
        <asp:GridView ID="gvStudents" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" OnRowDataBound="gvStudents_RowDataBound" DataKeyNames="id">
            <Columns>
                <asp:BoundField DataField="formatted_rollno" HeaderText="Roll No" />
                <asp:BoundField DataField="name" HeaderText="Name" />
                <asp:TemplateField HeaderText="Gender">
                    <ItemTemplate>
                        <asp:Label ID="lblGender" runat="server" Text='<%# Eval("gender") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Present">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkPresent" runat="server" AutoPostBack="true" OnCheckedChanged="chkPresent_CheckedChanged" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Absence">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkAbsence" runat="server" AutoPostBack="true" OnCheckedChanged="chkAbsence_CheckedChanged" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Text="Pending"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        
        <!-- Summary Labels -->
        <asp:Label ID="lblTotalStudents" runat="server" Text="Total Students: 0" CssClass="badge badge-secondary" Style="color: black;" />
        <asp:Label ID="lblTotalPresent" runat="server" Text="Total Present: 0" CssClass="badge badge-success" Style="color: black;" />
        <asp:Label ID="lblTotalAbsence" runat="server" Text="Total Absence: 0" CssClass="badge badge-danger" Style="color: black;" />
        <asp:Label ID="lblTotalMale" runat="server" Text="Total Male: 0" CssClass="badge badge-primary" Style="color: black;" />
        <asp:Label ID="lblTotalFemale" runat="server" Text="Total Female: 0" CssClass="badge badge-info" Style="color: black;" />
        
        <!-- Submit Button -->
        <asp:Button ID="btnSubmit" runat="server" Text="Submit Attendance" CssClass="btn btn-success" OnClientClick="return confirmSubmit();" OnClick="btnSubmit_Click" />
    </asp:Panel>

    <!-- Bootstrap Toast -->
    <div id="successToast" class="toast" style="position: absolute; top: 20px; right: 20px;">
        <div class="toast-header">
            <strong class="mr-auto">Notification</strong>
            <button type="button" class="ml-2 mb-1 close" data-dismiss="toast">&times;</button>
        </div>
        <div class="toast-body">
            Attendance data submitted successfully!
        </div>
    </div>

    <script>
        function confirmSubmit() {
            let totalPresent = 0;
            let totalAbsence = 0;
            let presentStudents = [];
            let absenceStudents = [];
            let uncheckedStudents = [];

            $('#<%= gvStudents.ClientID %> tr').each(function () {
                let row = $(this);
                let chkPresent = row.find('input[id$="chkPresent"]');
                let chkAbsence = row.find('input[id$="chkAbsence"]');
                let lblName = row.find('td:eq(1)').text(); // Get student name from the second column

                if (chkPresent.length && chkAbsence.length) {
                    if (chkPresent.is(':checked')) {
                        totalPresent++;
                        presentStudents.push(lblName);
                    } else if (chkAbsence.is(':checked')) {
                        totalAbsence++;
                        absenceStudents.push(lblName);
                    } else {
                        uncheckedStudents.push(lblName);
                    }
                }
            });

            let summary = `
                <p>Total Present: ${totalPresent}</p>
                <p>Total Absence: ${totalAbsence}</p>
                <p>Present Students: ${presentStudents.join(', ')}</p>
                <p>Absence Students: ${absenceStudents.join(', ')}</p>
            `;

            if (uncheckedStudents.length > 0) {
                summary += `<p>Unchecked Students: ${uncheckedStudents.join(', ')}</p>`;
            }

            Swal.fire({
                title: 'Attendance Summary',
                html: summary,
                icon: 'info',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Confirm and Submit'
            }).then((result) => {
                if (result.isConfirmed) {
                    __doPostBack('<%= btnSubmit.UniqueID %>', '');
                }
            });

            return false; // Prevent default postback
        }

        function showSuccessMessage() {
            $('#successToast').toast({ delay: 2000 });
            $('#successToast').toast('show');
        }
    </script>
</asp:Content>