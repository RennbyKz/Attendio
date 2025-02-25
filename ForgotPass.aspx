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
   font-family: 'Poppins', sans-serif;
    background-image: url('images/background.jpg');
    background-repeat: no-repeat;
    background-size: cover;
    color: #fff;
    margin: 0;
    padding: 0;
    height: 100vh;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    animation: fadeIn 1.5s ease-in-out;
}

@keyframes fadeIn {
    from { opacity: 0; }
    to { opacity: 1; }
}

.container {
    background-color: rgba(255, 255, 255, 0.8);
    padding: 40px;
    border-radius: 12px;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.5);
    max-width: 400px;
    width: 100%;
    text-align: center;
    animation: slideIn 1s ease-out;
}

@keyframes slideIn {
    from { transform: translateY(50px); opacity: 0; }
    to { transform: translateY(0); opacity: 1; }
}

h2 {
    font-size: 24px;
    color: #E94560;
    margin-bottom: 20px;
}

.form-group {
    margin-bottom: 15px;
    text-align: left;
}

label {
    display: block;
    margin-bottom: 5px;
    font-weight: bold;
    color: #333;
}

input {
    width: 100%;
    padding: 12px;
    border: 1px solid #ccc;
    border-radius: 8px;
    box-sizing: border-box;
    font-size: 14px;
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
    transition: background 0.3s ease;
}

button:hover {
    background-color: #d13350;
}

button:disabled {
    background-color: #ccc;
    cursor: not-allowed;
}

.hidden {
    display: none;
}
 .header {
     position: absolute;
     top: 20px;
     left: 40px; /* Adjust this to control spacing from the left */
     text-align: left;
 }

 .header h2 {
     margin: 0;
     font-size: 60px; /* Increase the main header text size */
     font-weight: bold;
     color: #ffffff;
 }

 .header h3 {
     margin: 10px 0 0;
     font-size: 50px; /* Increase the subheader text size */
     font-weight: 300;
 }

    </style>
</head>
<body>

        <!-- Header -->
        <div class="header">
            <h2>University Of Computer Studies</h2>
            <h3>Pathein</h3> 
        </div><br /><br />

<form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    <div class="container">
        <img src="images/UcspLogo1.png" alt="UcspLogo" style="height: 200px;"/>
        <h2>Forgot Password</h2>

        <div class="form-group" id="emailSection">
            <label for="txtEmail">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Enter your email"></asp:TextBox><br>
            <br><button type="button" onclick="sendOTP()">Send OTP</button>
        </div>

        <div class="form-group hidden" id="otpSection">
            <label for="txtOTP">OTP</label>
            <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control" Placeholder="Enter OTP"></asp:TextBox>
            <br>s<button type="button" onclick="verifyOTP()">Verify OTP</button>
        </div>

        <div class="form-group hidden" id="passwordSection">
            <label for="txtNewPassword">New Password</label>
            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Enter new password"></asp:TextBox>
            <label for="txtConfirmPassword">Confirm Password</label><br>
            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Confirm new password"></asp:TextBox><br>
             <br><button type="button" onclick="updatePassword()">Update Password</button><br>
            <br><button type="button" onclick="location.href='index.aspx'">Back to Login</button>
        </div>
    </div>
</form>

<script>
    function sendOTP() {
        const email = document.getElementById('<%= txtEmail.ClientID %>').value;
        fetch('ForgotPass.aspx/SendOTP', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email: email })
        })
        .then(response => response.json())
        .then(data => {
            if (data.d.startsWith("Error:")) {
                showError(data.d);
            } else {
                showSuccess(data.d);
                document.getElementById('emailSection').classList.add('hidden');
                document.getElementById('otpSection').classList.remove('hidden');
            }
        })
        .catch(error => showError(error));
    }

    function verifyOTP() {
        const otp = document.getElementById('<%= txtOTP.ClientID %>').value;
        fetch('ForgotPass.aspx/VerifyOTP', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ otp: otp, email: document.getElementById('<%= txtEmail.ClientID %>').value })
        })
        .then(response => response.json())
        .then(data => {
            if (data.d.startsWith("Error:")) {
                showError(data.d);
            } else {
                showSuccess(data.d);
                if (data.d === 'OTP verified') {
                    document.getElementById('otpSection').classList.add('hidden');
                    document.getElementById('passwordSection').classList.remove('hidden');
                }
            }
        })
        .catch(error => showError(error));
    }

    function updatePassword() {
        const newPassword = document.getElementById('<%= txtNewPassword.ClientID %>').value;
        const confirmPassword = document.getElementById('<%= txtConfirmPassword.ClientID %>').value;
        if (newPassword !== confirmPassword) {
            return showError('Passwords do not match');
        }
        fetch('ForgotPass.aspx/UpdatePassword', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                newPassword: newPassword,
                confirmPassword: confirmPassword,
                email: document.getElementById('<%= txtEmail.ClientID %>').value
            })
        })
        .then(response => response.json())
        .then(data => {
            if (data.d.startsWith("Error:")) {
                showError(data.d);
            } else {
                showSuccess(data.d);
            }
        })
        .catch(error => showError(error));
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
