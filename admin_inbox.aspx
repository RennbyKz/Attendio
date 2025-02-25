<%@ Page Title="" Language="C#" MasterPageFile="~/admindashboard.Master" AutoEventWireup="true" CodeBehind="admin_inbox.aspx.cs" Inherits="AMS.admin_inbox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .email-grid {
            width: 100%;
            border-collapse: collapse;
            font-family: Arial, sans-serif;
        }
        .email-grid th, .email-grid td {
            padding: 10px;
            border-bottom: 1px solid #ddd;
        }
        .email-grid th {
            background-color: #f1f1f1;
            text-align: left;
        }
        .email-grid tr:nth-child(even) {
            background-color: #f9f9f9;
        }
        .email-grid tr:hover {
            background-color: #f1f1f1;
        }
        .email-grid .email-subject {
            font-weight: bold;
            color: #333;
            cursor: pointer;
        }
        .email-grid .email-from {
            color: #555;
        }
        .email-grid .email-date {
            color: #999;
        }
    /* Envelope Theme Styles */
    .email-details {
        background-color: #f9f9f9;
        border: 1px solid #ddd;
        border-radius: 8px;
        padding: 20px;
        max-width: 600px;
        margin: 20px auto;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        font-family: 'Arial', sans-serif;
    }

    .email-details h2 {
        font-size: 24px;
        color: #333;
        margin-bottom: 15px;
        border-bottom: 2px solid #ddd;
        padding-bottom: 10px;
    }

    .email-details p {
        font-size: 16px;
        color: #555;
        margin: 10px 0;
    }

    .email-details p strong {
        color: #333;
    }

    .email-details .email-body {
        background-color: #fff;
        border: 1px solid #ddd;
        border-radius: 4px;
        padding: 15px;
        margin-top: 15px;
        font-size: 14px;
        color: #444;
        line-height: 1.6;
    }

    .submit-btn {
        background-color: #007bff;
        color: #fff;
        border: none;
        padding: 10px 20px;
        border-radius: 4px;
        font-size: 16px;
        cursor: pointer;
        margin-top: 20px;
        transition: background-color 0.3s ease;
    }

    .submit-btn:hover {
        background-color: #0056b3;
    }

    /* Envelope-like border effect */
    .email-details::before {
        content: '';
        position: absolute;
        top: -10px;
        left: 50%;
        transform: translateX(-50%);
        width: 20px;
        height: 20px;
        background-color: #f9f9f9;
        border: 1px solid #ddd;
        border-bottom: none;
        border-right: none;
        transform: rotate(45deg);
        box-shadow: -4px -4px 8px rgba(0, 0, 0, 0.1);
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="pnlInbox" runat="server" Visible="true">
        <asp:GridView ID="gvEmails" runat="server" AutoGenerateColumns="false" CssClass="email-grid">
            <Columns>
                <asp:TemplateField HeaderText="Subject">
                    <ItemTemplate>
                        <a class="email-subject" href="admin_inbox.aspx?emailId=<%# Eval("Id") %>"><%# Eval("Subject") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="From" Visible="false">
                    <ItemTemplate>
                        <span class="email-from"><%# Eval("From") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <span class="email-date"><%# Eval("Date") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlEmailDetails" runat="server" Visible="false">
        <div class="email-details">
            <h2 id="lblSubject" runat="server"></h2>
            <p><strong>Date:</strong> <span id="lblDate" runat="server"></span></p>
            <p><strong>From:</strong></p>
            <asp:Literal ID="lblBody" runat="server"></asp:Literal>
        </div>
        <asp:Button ID="btnBackToInbox" runat="server" Text="Back to Inbox" OnClick="BackToInbox_Click" CssClass="submit-btn" />
    </asp:Panel>
</asp:Content>
