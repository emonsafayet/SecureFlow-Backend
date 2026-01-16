using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using SecureFlow.Application.Common.Interfaces;

namespace SecureFlow.Infrastructure.Email;

public class SmtpEmailService : IEmailService
{
    private readonly SmtpSettings _settings;

    public SmtpEmailService(IOptions<SmtpSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendAsync(
        string to,
        string subject,
        string body,
        bool isHtml = true)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_settings.UserName),
            Subject = subject,
            Body = body,
            IsBodyHtml = isHtml
        };

        message.To.Add(to);

        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            Credentials = new NetworkCredential(
                _settings.UserName,
                _settings.Password),
            EnableSsl = _settings.EnableSsl
        };

        await client.SendMailAsync(message);
    }
}
