using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;

namespace AMS
{
    public partial class admin_inbox : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                   

                if (Request.QueryString["emailId"] != null)
                {
                    string emailId = Request.QueryString["emailId"];
                    LoadEmailDetails(emailId);
                }
                else
                {
                    LoadInbox();
                }
            }
        }

        private void LoadInbox()
        {
            List<EmailMessage> emails = FetchEmails();
            gvEmails.DataSource = emails;
            gvEmails.DataBind();
        }

        private void LoadEmailDetails(string emailId)
        {
            var email = FetchEmailById(emailId);
            if (email != null)
            {
                lblSubject.InnerText = email.Subject;
                lblDate.InnerText = email.Date;
                lblBody.Text = email.Body; // Render HTML content
                pnlEmailDetails.Visible = true;
                pnlInbox.Visible = false;
            }
        }

        private List<EmailMessage> FetchEmails()
        {
            List<EmailMessage> emails = new List<EmailMessage>();
            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, true);
                client.Authenticate("ucspstudentsaffair@gmail.com", "hlso ftks lrbl pzgy");

                client.Inbox.Open(MailKit.FolderAccess.ReadOnly);

                var uids = client.Inbox.Search(SearchQuery.All);
                foreach (var uid in uids)
                {
                    var message = client.Inbox.GetMessage(uid);
                    var body = message.HtmlBody ?? message.TextBody; // Use HtmlBody if available, otherwise fallback to TextBody
                    System.Diagnostics.Debug.WriteLine($"Email Body (UID: {uid}): {body}"); // Debug log

                    emails.Add(new EmailMessage
                    {
                        Id = uid.Id,
                        Subject = message.Subject,
                        From = message.From.ToString(),
                        Date = message.Date.ToString("dd/MM/yyyy HH:mm"),
                        Body = body
                    });
                }

                client.Disconnect(true);
            }
            return emails;
        }

        private EmailMessage FetchEmailById(string emailId)
        {
            EmailMessage email = null;
            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, true);
                client.Authenticate("ucspstudentsaffair@gmail.com", "hlso ftks lrbl pzgy");

                client.Inbox.Open(MailKit.FolderAccess.ReadOnly);

                var uid = new UniqueId(uint.Parse(emailId));
                var message = client.Inbox.GetMessage(uid);
                var body = message.HtmlBody ?? message.TextBody; // Use HtmlBody if available, otherwise fallback to TextBody
                System.Diagnostics.Debug.WriteLine($"Email Body (UID: {uid}): {body}"); // Debug log

                email = new EmailMessage
                {
                    Id = uid.Id,
                    Subject = message.Subject,
                    From = message.From.ToString(),
                    Date = message.Date.ToString("dd/MM/yyyy HH:mm"),
                    Body = body
                };

                client.Disconnect(true);
            }
            return email;
        }

        protected void BackToInbox_Click(object sender, EventArgs e)
        {
             Response.Redirect("admin_inbox.aspx");
        }

       
        public class EmailMessage
        {
            public uint Id { get; set; }
            public string Subject { get; set; }
            public string From { get; set; }
            public string Date { get; set; }
            public string Body { get; set; }
        }
    }
}
