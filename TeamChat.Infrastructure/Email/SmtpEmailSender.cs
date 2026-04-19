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

    public string BuildVerificationMessage(Guid userId, ref string? token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentNullException(nameof(token));

        var frontendUrl = _configuration["Frontend:Url"] ?? "http://localhost:5173";
        var encodedToken = Uri.EscapeDataString(token);
        var link = $"{frontendUrl}/verify-email?userId={userId}&token={encodedToken}";
        
        return BuildVerificationEmailHtml(link);
    }
    private static string BuildVerificationEmailHtml(string verificationLink) => $"""
    <!DOCTYPE html>
    <html lang="en">
    <head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"></head>
    <body style="margin:0;padding:24px 16px;background:#f4f4f0;font-family:-apple-system,'Segoe UI',Helvetica,Arial,sans-serif;">
      <div style="max-width:520px;margin:0 auto;">
        <div style="background:#ffffff;border-radius:16px;overflow:hidden;border:1px solid #e8e6e0;">

          <div style="background:#1a1a1a;padding:32px 40px 28px;">
            <span style="color:#ffffff;font-size:16px;font-weight:600;letter-spacing:-0.3px;">TeamChat</span>
          </div>

          <div style="padding:36px 40px 32px;">
            <h1 style="margin:0 0 8px;font-size:22px;font-weight:600;color:#1a1a1a;letter-spacing:-0.4px;line-height:1.25;">
              Confirm your email address
            </h1>
            <p style="margin:0 0 24px;font-size:15px;color:#6b6b6b;line-height:1.6;">
              You're almost there. Click the button below to complete your registration and start using TeamChat.
            </p>

            <a href="{verificationLink}" style="display:inline-block;background:#1a1a1a;color:#ffffff;text-decoration:none;font-size:14px;font-weight:500;padding:13px 28px;border-radius:10px;">
              Verify email →
            </a>

            <div style="margin:28px 0;border-top:1px solid #f0ede8;"></div>

            <p style="margin:0 0 6px;font-size:13px;color:#9b9b9b;">Button not working? Copy the link below:</p>
            <p style="margin:0 0 20px;font-size:12px;color:#b0aca5;word-break:break-all;font-family:monospace;background:#f8f7f4;padding:10px 12px;border-radius:8px;border:1px solid #eceae5;">
              {verificationLink}
            </p>

            <p style="margin:0;font-size:13px;color:#b0aca5;line-height:1.5;">
              This link expires in <strong style="color:#6b6b6b;">24 hours</strong>.
              If you didn't create an account, you can safely ignore this email.
            </p>
          </div>

          <div style="background:#faf9f7;border-top:1px solid #f0ede8;padding:18px 40px;">
            <p style="margin:0;font-size:12px;color:#b0aca5;">© 2025 TeamChat · <a href="#" style="color:#b0aca5;">Unsubscribe</a> · <a href="#" style="color:#b0aca5;">Privacy Policy</a></p>
          </div>

        </div>
      </div>
    </body>
    </html>
    """;

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