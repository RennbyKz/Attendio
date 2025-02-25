<%@ Page Title="" Language="C#" MasterPageFile="~/admindashboard.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="AMS.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>

/* Card Widget Styles */
.card-stats {
    border-radius: 10px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    transition: transform 0.3s ease;
}

.card-stats:hover {
    transform: translateY(-5px);
}

.card-stats .card-body {
    display: flex;
    align-items: center;
    justify-content: space-between;
}

.card-stats .icon-big {
    font-size: 3rem;
}

.card-stats .numbers {
    text-align: right;
}

.card-stats .card-category {
    font-size: 1rem;
    font-weight: bold;
    color: #000000;
}

.card-stats .card-title {
    font-size: 1.5rem;
    font-weight: bold;
    color: #000000;
}
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
   <link rel="stylesheet" href=""css/all.min.css" / ><br />
    <div class="container">
        <div class="row">
            <div class="col-md-3">
                <div class="card card-stats card-round" onclick="location.href='ManageStudents.aspx';" style="cursor: pointer; background-color: #809671;">
                    <div class="card-body">
                        <div class="row align-items-center">
                            <div class="col-icon">
                                <div class="icon-big text-center icon-primary bubble-shadow-small">
                                    <i class="fas fa-users"></i>
                                </div>
                            </div>
                            <div class="col col-stats ms-3 ms-sm-0">
                                <div class="numbers">
                                    <p class="card-category">Total Students</p>
                                    <h4 class="card-title"><asp:Label ID="lblTotalStudents" runat="server" Text="0"></asp:Label></h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card card-stats card-round" onclick="location.href='ManageTeachers.aspx';" style="cursor: pointer; background-color: #e5d2b8;">
                    <div class="card-body">
                        <div class="row align-items-center">
                            <div class="col-icon">
                                <div class="icon-big text-center icon-info bubble-shadow-small">
                                    <i class="fas fa-chalkboard-teacher"></i>
                                </div>
                            </div>
                            <div class="col col-stats ms-3 ms-sm-0">
                                <div class="numbers">
                                    <p class="card-category">Total Teachers</p>
                                    <h4 class="card-title"><asp:Label ID="lblTotalTeachers" runat="server" Text="0"></asp:Label></h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card card-stats card-round" onclick="location.href='ManageEntities.aspx';" style="cursor: pointer; background-color: #725c3a;">
                    <div class="card-body">
                        <div class="row align-items-center">
                            <div class="col-icon">
                                <div class="icon-big text-center icon-success bubble-shadow-small">
                                    <i class="fas fa-building"></i>
                                </div>
                            </div>
                            <div class="col col-stats ms-3 ms-sm-0">
                                <div class="numbers">
                                    <p class="card-category">Total Departments</p>
                                    <h4 class="card-title"><asp:Label ID="lblTotalDepartments" runat="server" Text="0"></asp:Label></h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card card-stats card-round" onclick="location.href='ManageEntities.aspx';" style="cursor: pointer; background-color: #b3b792;">
                    <div class="card-body">
                        <div class="row align-items-center">
                            <div class="col-icon">
                                <div class="icon-big text-center icon-secondary bubble-shadow-small">
                                    <i class="fas fa-book"></i>
                                </div>
                            </div>
                            <div class="col col-stats ms-3 ms-sm-0">
                                <div class="numbers">
                                    <p class="card-category">Total Subjects</p>
                                    <h4 class="card-title"><asp:Label ID="lblTotalSubjects" runat="server" Text="0"></asp:Label></h4>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
     <footer>@Copyright 2025 Student Affair</footer>
</asp:Content>
