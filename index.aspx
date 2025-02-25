<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="AMS.index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UCSP Rollcall Attendance System</title>
    <link rel="icon" type="image/png" href="images/UcspLogo1.png" />        
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Bebas+Neue&display=swap');
        body {
            font-family: 'Poppins', sans-serif;
            background-image: url('images/background.jpg'); 
            background-repeat: no-repeat; /* Optional */ 
            background-size: cover; /* Optional */
            color: #fff;
            margin: 0;
            padding: 0;
            height: 100vh;
            display: flex;
            flex-direction: column;
            animation: fadeIn 1.5s ease-in-out; /* Add fade-in animation to the body */
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
            left: 40px; /* Adjust this to control spacing from the left */
            text-align: left;
        }

        .header h2 {
            margin: 0;
            font-size: 60px; /* Increase the main header text size */
            font-weight: bold;
        }

        .header h3 {
            margin: 10px 0 0;
            font-size: 50px; /* Increase the subheader text size */
            font-weight: 300;
        }

        .container {
            margin: 140px auto; /* Center the form container vertically and horizontally */
            background-color: rgba(255, 255, 255, 0.7); /* Adjust the alpha value (0.8) for desired transparency */
            padding: 40px;
            border-radius: 12px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.5);
            max-width: 400px;
            width: 100%;
            display: flex;
            flex-direction: column;
            align-items: center;
            animation: slideIn 1s ease-out; /* Add slide-in animation to the form container */
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

        .login-form h1 {
            text-align: center;
            margin-bottom: 20px;
            margin-top: 8px;
            font-size: 24px;
            color: #E94560;
        }

        .login-form label {
            display: block;
            margin-bottom: 8px;
            font-weight: bold;
            color: #333;
        }

        .login-form input[type="text"],
        .login-form input[type="password"] {
            width: 100%;
            padding: 12px;
            margin-bottom: 16px;
            border: 1px solid #ccc;
            border-radius: 8px;
            box-sizing: border-box;
        }

        .login-form input[type="submit"] {
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

        .login-form input[type="submit"]:hover {
            background-color: #d13350;
        }

        .login-form .links {
            text-align: center;
            margin-top: 10px;
        }

        .login-form .links a {
            color: #00A8CC;
            text-decoration: none;
        }

        .login-form .links a:hover {
            text-decoration: underline;
        }
        .btn{
               background: linear-gradient(to bottom, #00A9F2, #00E2E0) !important;
        }
       .links a {
    color: red !important;
    text-decoration: none !important;
    font-size: 16px;
}

.links a:hover {
    text-decoration: none !important;
    color: green;
}

    </style>
    <!-- SweetAlert2 CSS -->
    <link href="css/sweetalert2.min.css" rel="stylesheet" />
    <!-- SweetAlert2 JS -->
    <script src="js/sweetalert2@11.js"></script>
</head>

<body>
    <!-- Header -->
    <div class="header">
        <h2>University Of Computer Studies</h2>
        <h3>Pathein</h3> 
    </div><br /><br />

    <!-- Form Container -->
    <form id="form1" runat="server">
        <div class="container">
            <img src="images/UcspLogo1.png" alt="UcspLogo" />
            <div class="login-form">
                <h1 style="color: #00a9f2;">Member Login</h1>
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message" Visible="false"></asp:Label>
                <label for="username">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Placeholder="Enter your username"></asp:TextBox>
                <label for="password">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" Placeholder="Enter your password"></asp:TextBox>
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
                <div class="links">
                    <a href="ForgotPass.aspx">Forgot Username / Password?</a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>