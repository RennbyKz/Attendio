<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Changeme.aspx.cs" Inherits="AMS.Changeme" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Change Username & Password</title>

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
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div class="header">
        <h2>University Of Computer Studies</h2>
        <h3>Pathein</h3> 
    </div><br /><br />

    <div class="container">
        <img src="images/UcspLogo1.png" alt="UcspLogo" />
       <h2 style="color: red;">Change Username & Password</h2>

        <div class="form-group">
            <label for="textNewUsername">Username</label>
            <input type="text" id="textNewUsername" runat="server" name="textNewUsername" onkeyup="checkUsername()" required>
            <span id="usernameError" class="error"></span>
        </div>

        <div class="form-group">
            <label for="txtNewPassword">Password</label>
            <input type="password" id="txtNewPassword" runat="server" name="txtNewPassword" onkeyup="validatePassword()" required>
            <span id="passwordError" class="error"></span>
        </div>

        <div class="form-group">
            <label for="txtConfirmPassword">Confirm Password</label>
            <input type="password" id="txtConfirmPassword" runat="server" name="txtConfirmPassword" onkeyup="validatePasswordMatch()" required>
            <span id="confirmPasswordError" class="error"></span>
        </div>

        <button id="btnChangePassword" runat="server" onserverclick="btnChangePassword_Click" disabled>Change Password</button>
    </div>
</form>

<script>
    function checkUsername() {
        let username = document.getElementById("textNewUsername").value;
        let usernameError = document.getElementById("usernameError");

        if (username.length < 3) {
            usernameError.innerHTML = "<span style='color: red;'>Username must be at least 3 characters</span>";
            disableSubmit();
            return;
        }

        // AJAX request to check username availability
        let xhr = new XMLHttpRequest();
        xhr.open("POST", "Changeme.aspx/CheckUsername", true);
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

    function validatePassword() {
        let password = document.getElementById("txtNewPassword").value;
        let passwordError = document.getElementById("passwordError");

        if (password.length < 6) {
            passwordError.innerHTML = "Password must be at least 6 characters";
            disableSubmit();
        } else {
            passwordError.innerHTML = "";
            enableSubmit();
        }
    }

    function validatePasswordMatch() {
        let password = document.getElementById("txtNewPassword").value;
        let confirmPassword = document.getElementById("txtConfirmPassword").value;
        let confirmPasswordError = document.getElementById("confirmPasswordError");

        if (password !== confirmPassword) {
            confirmPasswordError.innerHTML = "Password doesn't match";
            disableSubmit();
        } else {
            confirmPasswordError.innerHTML = "";
            enableSubmit();
        }
    }

    function enableSubmit() {
        let usernameError = document.getElementById("usernameError").innerHTML;
        let passwordError = document.getElementById("passwordError").innerHTML;
        let confirmPasswordError = document.getElementById("confirmPasswordError").innerHTML;
        let username = document.getElementById("textNewUsername").value;
        let password = document.getElementById("txtNewPassword").value;
        let confirmPassword = document.getElementById("txtConfirmPassword").value;

        if (username.length >= 3 && password.length >= 6 && password === confirmPassword &&
            usernameError.indexOf("taken") === -1 && passwordError === "" && confirmPasswordError === "") {
            document.getElementById("btnChangePassword").disabled = false;
        } else {
            document.getElementById("btnChangePassword").disabled = true;
        }
    }

    function disableSubmit() {
        document.getElementById("btnChangePassword").disabled = true;
    }
</script>

</body>
</html>
