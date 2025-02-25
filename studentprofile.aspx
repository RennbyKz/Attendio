<%@ Page Title="" Language="C#" MasterPageFile="~/students.Master" AutoEventWireup="true" CodeBehind="studentprofile.aspx.cs" Inherits="AMS.studentprofile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <style>
            body {
                font-family: 'Arial', sans-serif;
                background-color: #f0f2f5;
                color: #333;
            }

            .profile-header {
                text-align: center;
                margin-bottom: 30px;
            }

            .profile-header img {
                border-radius: 50%;
                width: 200px; /* Large Circular Profile */
                height: 200px;
                object-fit: cover;
                border: 5px solid #fff;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                margin-bottom: 15px;
            }

            .card {
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                border-radius: 10px;
                background-color:#e5d2b8;
                padding: 20px;
                margin-bottom: 20px;
            }

            .card-body {
                padding: 2rem;
            }

            .card-title {
                font-size: 1.5rem;
                color: #007bff;
                margin-bottom: 1rem;
                text-align: center;
            }

            .profile-table {
                width: 100%;
                border-collapse: collapse;
                margin-top: 15px;
            }

            .profile-table th {
                text-align: left;
                font-weight: bold;
                width: 150px;
                color: #007bff;
                padding: 8px 0;
            }

            .profile-table td {
                padding: 8px 0;
            }

            .btn-primary {
               background: linear-gradient(to bottom, #00e2e0, #aae5f2); 
                border-color: #007bff;
            }
      

            .btn-primary:hover {
                background-color: #0056b3;
                border-color: #0056b3;
            }

            .file-upload {
                display: flex;
                justify-content: center;
                align-items: center;
                margin-top: 20px;
            }

            .file-upload input[type="file"] {
                display: none;
            }

            .custom-file-upload {
                border: 1px solid #007bff;
                display: inline-block;
                padding: 8px 12px;
                cursor: pointer;
                border-radius: 5px;
                color: #007bff;
            }

            .custom-file-upload:hover {
                background-color: #007bff;
                color: #fff;
            }

            .container {
                max-width: 900px;
                margin: auto;
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

        <div class="row">
            <!-- Profile Picture Section -->
            <div class="col-md-4">
                <div class="card">
                    <div class="card-body text-center">
                        <asp:Image ID="imgProfile" runat="server" CssClass="profile-header img-fluid" />
                        <asp:FileUpload ID="fileUpload" runat="server" CssClass="choosefile" />
                        <asp:Button ID="btnUpload" runat="server" Text="Upload Photo" CssClass="btn btn-primary mt-2" OnClick="btnUpload_Click" />
                    </div>
                </div>
            </div>

            <!-- Profile Details Section -->
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <h3 class="card-title">Student Profile</h3>
                        <table class="profile-table">
                            <tr>
                                <th>Name:</th>
                                <td><asp:Label ID="lblName" runat="server" /></td>
                            </tr>
                            <tr>
                                <th>Semester:</th>
                                <td><asp:Label ID="lblSemester" runat="server" /></td>
                            </tr>
                            <tr>
                                <th>Roll No:</th>
                                <td><asp:Label ID="lblRollNo" runat="server" /></td>
                            </tr>
                            <tr>
                                <th>Section:</th>
                                <td><asp:Label ID="lblSection" runat="server" /></td>
                            </tr>
                            <tr>
                                <th>Gender:</th>
                                <td><asp:Label ID="lblGender" runat="server" /></td>
                            </tr>
                            <tr>
                                <th>Email:</th>
                                <td><asp:Label ID="lblEmail" runat="server" /></td>
                            </tr>
                            <tr>
                                <th>Major:</th>
                                <td><asp:Label ID="lblMajor" runat="server" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <footer>@Copyright 2025 Student Affair</footer>
</asp:Content>
