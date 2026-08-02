using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ruba10828tl
{
    public partial class Contact : System.Web.UI.Page
    {
        // تُقرأ القيم من Web.config
        private static readonly string ToEmail = ConfigurationManager.AppSettings["ContactToEmail"];
        private static readonly string FromEmail = ConfigurationManager.AppSettings["ContactFromEmail"]; // نفس حساب SMTP

        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnSend_OnClick(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            // تحقّق من إعدادات Web.config حتى لا يرجع Null
            if (string.IsNullOrWhiteSpace(ToEmail))
            {
                lblStatus.CssClass = "msg-err";
                lblStatus.Text = "Configuration error: ContactToEmail is missing in Web.config.";
                return;
            }
            if (string.IsNullOrWhiteSpace(FromEmail))
            {
                lblStatus.CssClass = "msg-err";
                lblStatus.Text = "Configuration error: ContactFromEmail is missing in Web.config.";
                return;
            }

            try
            {
                var msg = new MailMessage
                {
                    From = new MailAddress(FromEmail), // يجب أن يطابق حساب SMTP
                    Subject = txtSubject.Text.Trim(),
                    IsBodyHtml = false,
                    Body = $"From: {txtFrom.Text}\r\n\r\n{txtBody.Text}"
                };

                // المستلم (من Web.config)
                msg.To.Add(ToEmail);

                // الرد يوجّه لبريد المستخدم
                if (!string.IsNullOrWhiteSpace(txtFrom.Text))
                    msg.ReplyToList.Add(new MailAddress(txtFrom.Text));

                // المرفقات إن وجدت
                if (fuFiles.HasFiles)
                {
                    foreach (var posted in fuFiles.PostedFiles)
                    {
                        var file = posted as HttpPostedFile;
                        if (file == null || file.ContentLength == 0) continue;

                        var att = new Attachment(file.InputStream, file.FileName);
                        msg.Attachments.Add(att);
                    }
                }

                // يستخدم إعدادات SMTP من Web.config تلقائياً
                using (var smtp = new SmtpClient())
                {
                    // لو SMTP مؤسسي لا يحتاج SSL، غيّري حسب مزوّدك
                    smtp.EnableSsl = true;
                    smtp.Send(msg);
                }

                lblStatus.CssClass = "msg-ok";
                lblStatus.Text = "Your message has been sent successfully.";
                ClearForm();
            }
            catch (SmtpException ex)
            {
                lblStatus.CssClass = "msg-err";
                lblStatus.Text = "SMTP error: " + ex.Message;
            }
            catch (Exception ex)
            {
                lblStatus.CssClass = "msg-err";
                lblStatus.Text = "Failed to send: " + ex.Message;
            }
        }

        private void ClearForm()
        {
            txtFrom.Text = string.Empty;
            txtSubject.Text = string.Empty;
            txtBody.Text = string.Empty;
        }
    }
}