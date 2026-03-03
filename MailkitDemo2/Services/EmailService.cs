using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailkitDemo2.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
        Console.WriteLine($"SMTP SERVER: {_settings.SmtpServer}");
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(
            _settings.SenderName,
            _settings.SenderEmail));

        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        message.Body = new TextPart(isHtml ? "html" : "plain")
        {
            Text = body
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            _settings.SmtpServer,
            _settings.SmtpPort,
            SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(
            _settings.Username,
            _settings.Password);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, string attachmentPath)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(
            _settings.SenderName,
            _settings.SenderEmail));

        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        var builder = new BodyBuilder
        {
            HtmlBody = body
        };

        if (File.Exists(attachmentPath))
        {
            builder.Attachments.Add(attachmentPath);
        }

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();

        await client.ConnectAsync(
            _settings.SmtpServer,
            _settings.SmtpPort,
            SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(
            _settings.Username,
            _settings.Password);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}