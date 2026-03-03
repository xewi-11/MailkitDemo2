namespace MailkitDemo2.Interfaces
{


    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true);
        Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, string attachmentPath);
    }
}
