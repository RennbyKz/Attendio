<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPass.aspx.cs" Inherits="AMS.ForgotPass" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Forgot Password</title>

    <!-- SweetAlert2 -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <style>
        body {
            font-family: 'Arial', sans-serif;
            background-image: url('images/background.jpg');
            background-repeat: no-repeat;
            background-size: cover;
            color: #fff;
            margin: 0;
            padding: 0;
            height: 100vh;
            display: flex;
            flex-direction: column;
            animation: fadeIn 1.5s ease-in-out;
        }

        @keyframes fadeIn {
            from {
                opacity: 0;
            }
            to {
                opacity: 1;
            }
        }

        .header {
            position: absolute;
            top: 20px;
            left: 40px;
            text-align: left;
        }

        .header h2 {
            margin: 0;
            font-size: 48px;
            font-weight: bold;
        }

        .header h3 {
            margin: 10px 0 0;
            font-size: 38px;
            font-weight: 300;
        }

        .container {
            margin: 140px auto;
            background-color: rgba(255, 255, 255, 0.7);
            padding: 40px;
            border-radius: 12px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.5);
            max-width: 400px;
            width: 100%;
            display: flex;
            flex-direction: column;
            align-items: center;
            animation: slideIn 1s ease-out;
        }

        @keyframes slideIn {
            from {
                transform: translateY(50px);
                opacity: 0;
            }
            to {
                transform: translateY(0);
                opacity: 1;
            }
        }

        .container img {
            max-width: 150px;
            margin-bottom: 20px;
        }

        .form-group {
            margin-bottom: 15px;
            width: 100%;
        }

        label {
            display: block;
            margin-bottom: 8px;
            font-weight: bold;
            color: #333;
        }

        input {
            width: 100%;
            padding: 12px;
            margin-bottom: 16px;
            border: 1px solid #ccc;
            border-radius: 8px;
            box-sizing: border-box;
        }

        button {
            width: 100%;
            padding: 12px;
            background-color: #E94560;
            border: none;
            border-radius: 8px;
            color: #fff;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
        }

        button:disabled {
            background-color: #cccccc;
            cursor: not-allowed;
        }

        .error {
            color: #a94442;
            font-size: 14px;
            margin-top: 5px;
        }
    </style>
</head>
<body>

<form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    <div class="header">
        <h2>University Of Computer Studies</h2>
        <h3>Pathein</h3>
    </div><br /><br />

    <div class="container">
        <img src="images/UcspLogo1.png" alt="UcspLogo" />
        <h2>Forgot Password</h2>

        <div class="form-group">
            <label for="txtEmail">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Enter your email"></asp:TextBox>
            <span id="emailError" class="error"></span>
        </div>

        <button id="btnSendOTP" runat="server" onserverclick="btnSendOTP_Click">Send OTP</button>

        <div class="form-group" id="otpSection" style="display:none;">
            <label for="txtOTP">OTP</label>
            <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control" Placeholder="Enter OTP"></asp:TextBox>
            <span id="otpError" class="error"></span>
        </div>

        <button id="btnVerifyOTP" runat="server" onserverclick="btnVerifyOTP_Click" style="display:none;">Verify OTP</button>

        <div class="form-group" id="passwordSection" style="display:none;">
            <label for="txtNewPassword">New Password</label>
            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Enter new password"></asp:TextBox>
            <label for="txtConfirmPassword">Confirm Password</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Confirm new password"></asp:TextBox>
            <span id="passwordError" class="error"></span>
        </div>

        <button id="btnUpdatePassword" runat="server" onserverclick="btnUpdatePassword_Click" style="display:none;">Update Password</button>
    </div>
</form>

<script>
    function showOTPSection() {
        console.log("showOTPSection called");
        document.getElementById("otpSection").style.display = "block";
        document.getElementById("btnVerifyOTP").style.display = "block";
    }

    function showPasswordSection() {
        console.log("showPasswordSection called");
        document.getElementById("passwordSection").style.display = "block";
        document.getElementById("btnUpdatePassword").style.display = "block";
    }

    function showError(message) {
        Swal.fire('Error!', message, 'error');
    }

    function showSuccess(message) {
        Swal.fire('Success!', message, 'success');
    }
</script>

</body>
</html>