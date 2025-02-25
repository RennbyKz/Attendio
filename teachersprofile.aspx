<%@ Page Title="" Language="C#" MasterPageFile="~/teachers.Master" AutoEventWireup="true" CodeBehind="teachersprofile.aspx.cs" Inherits="AMS.teachersprofile" %>
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
            <div class="col-md-4">
                <div class="card">
                    <div class="card-body text-center">
                        <asp:Image ID="imgProfile" runat="server" CssClass="img-fluid rounded-circle mb-3" Width="150px" />
                        <asp:FileUpload ID="fileUpload" runat="server" />
                        <asp:Button ID="btnUpload" runat="server" Text="Upload Photo" CssClass="btn btn-primary mt-2" OnClick="btnUpload_Click" />
                    </div>
                </div>
            </div>
            <div class="col-md-8">
                <div class="card">
                    <div class="card-body">
                        <h3 class="card-title">Teacher Profile</h3>
                        <p class="card-text"><strong>Full Name:</strong> <asp:Label ID="lblFullName" runat="server" /></p>
                        <p class="card-text"><strong>Email:</strong> <asp:Label ID="lblEmail" runat="server" /></p>
                        <p class="card-text"><strong>Phone Number:</strong> <asp:Label ID="lblPhoneNumber" runat="server" /></p>
                        <p class="card-text"><strong>Department:</strong> <asp:Label ID="lblDepartment" runat="server" /></p>
                        <p class="card-text"><strong>Room:</strong> <asp:Label ID="lblRoom" runat="server" /></p>
                        <p class="card-text"><strong>Status:</strong> <asp:Label ID="lblStatus" runat="server" /></p>
                    </div>
                </div>
            </div>
        </div>
    </div>
   <footer>@Copyright 2025 Student Affair</footer>
</asp:Content>