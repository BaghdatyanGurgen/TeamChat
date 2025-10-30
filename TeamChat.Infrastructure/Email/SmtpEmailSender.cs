using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using TeamChat.Application.Abstraction.Infrastructure.Email;

namespace TeamChat.Infrastructure.Email;

public class SmtpEmailSender(IOptions<SmtpSettings> options, IConfiguration configuration) : IEmailSender
{
    private readonly SmtpSettings _settings = options.Value;
    private readonly IConfiguration _configuration = configuration;

    public string BuildVerificationLink(Guid userId, ref string? token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentNullException(nameof(token));

        var frontendUrl = _configuration["Frontend:Url"] ?? "http://localhost:5173";
        var encodedToken = Uri.EscapeDataString(token);

        return $"{frontendUrl}/verify-email?userId={userId}&token={encodedToken}";
    }


    public async Task SendEmailAsync(string? to, string subject, string htmlMessage)
    {
        if (string.IsNullOrEmpty(to))
            throw new ArgumentNullException(nameof(to));

        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.Username, _settings.Password),
            EnableSsl = _settings.EnableSsl
        };

        var mailMessage = new MailMessage(_settings.FromEmail, to, subject, htmlMessage)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(mailMessage);
    }
}