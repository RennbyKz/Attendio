<%@ Page Title="" Language="C#" MasterPageFile="~/students.Master" AutoEventWireup="true" CodeBehind="studentsetting.aspx.cs" Inherits="AMS.studentsetting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <style>
            body {
                font-family: 'Arial', sans-serif;
                background-color: #f8f9fa;
                margin: 0;
                padding: 0;
                height: 1000px;
            }

            .container {
                max-width: 600px;
                margin: 0 auto;
                padding: 20px;
                background-color: #fff;
                border-radius: 8px;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            }

            h2 {
                color: #343a40;
                font-weight: bold;
            }

            .card {
                background-color: #fff;
                border: 1px solid #dee2e6;
                border-radius: 8px;
                padding: 20px;
                margin-bottom: 20px;
            }

            .card-title {
                font-size: 1.25rem;
                font-weight: bold;
                color: #343a40;
                margin-bottom: 15px;
            }

            .form-group {
                margin-bottom: 15px;
            }

            .form-control {
                border-radius: 8px;
                border: 1px solid #ced4da;
                padding: 10px;
                font-size: 1rem;
            }

            .btn-primary {
                background-color: #007bff;
                border-color: #007bff;
                border-radius: 8px;
                padding: 10px 20px;
                font-size: 1rem;
                font-weight: bold;
            }

            .btn-primary:hover {
                background-color: #0056b3;
                border-color: #004085;
            }

            .text-danger {
                color: #dc3545 !important;
            }

            .text-success {
                color: #28a745 !important;
            }

            .text-primary {
                color: #007bff !important;
            }

            .mt-2 {
                margin-top: 0.5rem !important;
            }

            .mt-3 {
                margin-top: 1rem !important;
            }

            .mt-5 {
                margin-top: 3rem !important;
            }

            .shadow-sm {
                box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075) !important;
            }
        </style>
        <h2>Change Username & Password</h2>

        <!-- Change Username Section -->
        <div class="card p-4 mt-3">
            <h4>Change Username</h4>
            <asp:Label ID="lblCurrentUsername" runat="server" Text="Current Username: " CssClass="form-label"></asp:Label>
            <asp:TextBox ID="txtNewUsername" runat="server" CssClass="form-control" placeholder="Enter New Username" onkeyup="checkUsername()" required></asp:TextBox>
            <span id="usernameError" class="text-danger mt-2"></span>
            <asp:Button ID="btnChangeUsername" runat="server" CssClass="btn btn-primary mt-2" Text="Change Username" OnClick="btnChangeUsername_Click" Enabled="false" />
            <asp:Label ID="lblUserMsg" runat="server" CssClass="text-success mt-2"></asp:Label>
        </div>

        <!-- Change Password Section -->
        <div class="card p-4 mt-3">
            <h4>Change Password</h4>
            <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Current Password"></asp:TextBox>
            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control mt-2" placeholder="New Password"></asp:TextBox>
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control mt-2" placeholder="Confirm New Password"></asp:TextBox>
            <asp:Button ID="btnChangePassword" runat="server" CssClass="btn btn-primary mt-2" Text="Change Password" OnClick="btnChangePassword_Click" />
            <asp:Label ID="lblPassMsg" runat="server" CssClass="text-danger mt-2"></asp:Label>
        </div>

        <!-- Forgot Password Link -->
        <div class="mt-3">
            <a href="ForgotPass.aspx" class="text-primary">Forgot Password?</a>
        </div>
    </div>

    <script>
        function checkUsername() {
            let username = document.getElementById("<%= txtNewUsername.ClientID %>").value;
            let usernameError = document.getElementById("usernameError");

            if (username.length < 3) {
                usernameError.innerHTML = "<span style='color: red;'>Username must be at least 3 characters</span>";
                disableSubmit();
                return;
            }

            // AJAX request to check username availability
            let xhr = new XMLHttpRequest();
            xhr.open("POST", "studentsetting.aspx/CheckUsername", true);
            xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    let response = JSON.parse(xhr.responseText);
                    if (response.d === "taken") {
                        usernameError.innerHTML = "<span style='color: red;'>Username is already taken</span>";
                        disableSubmit();
                    } else {
                        usernameError.innerHTML = "<span style='color: green;'>Username is available</span>";
                        enableSubmit();
                    }
                }
            };
            xhr.send(JSON.stringify({ username: username }));
        }

        function enableSubmit() {
            document.getElementById("<%= btnChangeUsername.ClientID %>").disabled = false;
        }

        function disableSubmit() {
            document.getElementById("<%= btnChangeUsername.ClientID %>").disabled = true;
        }
    </script>
</asp:Content>



