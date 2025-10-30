namespace TeamChat.Application.Abstraction.Infrastructure.Email;

public interface IEmailSender
{
    string BuildVerificationLink(Guid id, ref string? token);
    Task SendEmailAsync(string? to, string subject, string htmlMessage);
}