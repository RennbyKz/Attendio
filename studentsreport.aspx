<%@ Page Title="" Language="C#" MasterPageFile="~/students.Master" AutoEventWireup="true" CodeBehind="studentsreport.aspx.cs" Inherits="AMS.studentsreport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <head>
        <!-- SweetAlert CSS -->
        <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet">
        
        <!-- SweetAlert JS -->
        <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    </head>
    <br /><br />
    <div class="envelope-border">
         <img src="images/UcspLogo1.png" alt="UcspLogo" style="height: 150px;"/>
        <h2 class="title">Contact To Student Affairs</h2>
        <table class="contact-table">
            <tr>
                <td><label for="txtName">Name <span class="required">*</span></label></td>
                <td><asp:TextBox ID="txtName" runat="server" CssClass="input-field" ReadOnly="true"></asp:TextBox></td>
            </tr>
            <tr>
                <td><label for="txtRollNo">Roll No. <span class="required">*</span></label></td>
                <td><asp:TextBox ID="txtRollNo" runat="server" CssClass="input-field" ReadOnly="true"></asp:TextBox></td>
            </tr>
            <tr>
                <td><label for="txtSection">Section <span class="required">*</span></label></td>
                <td><asp:TextBox ID="txtSection" runat="server" CssClass="input-field" ReadOnly="true"></asp:TextBox></td>
            </tr>
            <tr>
                <td><label for="txtEmail">E-mail <span class="required">*</span></label></td>
                <td><asp:TextBox ID="txtEmail" runat="server" CssClass="input-field" ReadOnly="true"></asp:TextBox></td>
            </tr>
            <tr>
                <td><label for="txtTo">To <span class="required">*</span></label></td>
                <td><asp:TextBox ID="txtTo" runat="server" CssClass="input-field" ReadOnly="true" Text="ucspstudentsaffair@gmail.com"></asp:TextBox></td>
            </tr>
            <tr>
                <td><label for="txtSubject">Subject <span class="required">*</span></label></td>
                <td><asp:TextBox ID="txtSubject" runat="server" CssClass="input-field"></asp:TextBox></td>
            </tr>
            <tr>
                <td><label for="txtMessage">Message <span class="required">*</span></label></td>
                <td><asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" CssClass="textarea-field"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Mail Now!" CssClass="submit-btn" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
    </div>
    <footer>&copy; 2025 Student Affair</footer>
    <style>
        .body {
            height: 1300px;
        }
        /* Envelope Border */
        .envelope-border {
            width: 50%;
            margin: auto;
            padding: 20px;
            border: 15px solid transparent;
            border-image: url('images/airmail-border.png') 30 round;
            background-color: white;
            text-align: center;
        }

        /* Title */
        .title {
            color: #9C6A6A;
            font-family: Arial, sans-serif;
        }

        /* Contact Table */
        .contact-table {
            width: 80%;
            margin: auto;
        }

        /* Labels */
        label {
            font-weight: bold;
        }

        /* Required Field Indicator */
        .required {
            color: red;
        }

        /* Input Fields */
        .input-field, .textarea-field {
            width: 250px;
            padding: 8px;
            border: 1px solid blue !important;
            border-radius: 5px;
            font-size: 14px;
        }

        /* Textarea */
        .textarea-field {
            height: 80px;
            width: 250px;
        }

        /* Submit Button */
        .submit-btn {
            background-color: red;
            border: none;
            padding: 10px 15px;
            color: white;
            font-size: 14px;
            cursor: pointer;
            border-radius: 5px;
        }

        .submit-btn:hover {
            background-color: #B76E79;
        }

        /* Footer */
        footer {
            margin-top: 20px;
            padding: 10px;
            background-color: #f1f1f1;
            border-top: 1px solid #e4e4e4;
            text-align: center;
            font-size: 20px;
            color: #555;
        }
    </style>
</asp:Content>
