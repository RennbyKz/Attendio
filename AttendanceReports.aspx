<%@ Page Title="" Language="C#" MasterPageFile="~/admindashboard.Master" AutoEventWireup="true" CodeBehind="AttendanceReports.aspx.cs" Inherits="AMS.AttendanceReports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Attendance Reports</h2>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
    <asp:DropDownList ID="ddlSemester" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSemester_SelectedIndexChanged">
        <asp:ListItem Text="Select Semester" Value="" />
       
    </asp:DropDownList>
    
    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" Placeholder="Select Date" AutoPostBack="true" OnTextChanged="txtDate_TextChanged"></asp:TextBox>
    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDate" Format="yyyy-MM-dd" />

    <asp:GridView ID="gvAttendanceReport" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
        <Columns>
            <asp:BoundField DataField="FormattedRollNo" HeaderText="Roll No" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Gender" HeaderText="Gender" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
        </Columns>
    </asp:GridView>
</asp:Content>