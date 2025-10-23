namespace TeamChat.Infrastructure.Email;

public interface IEmailSender
{
    string BuildVerificationLink(Guid id, string? email);
    Task SendEmailAsync(string? to, string subject, string htmlMessage);
}