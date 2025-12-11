using System.Net.Mail;

namespace TeamChat.Application.Validation;

public static class UserValidation
{
    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    internal static bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (password.Length < 8)
            return false;

        if (!password.Any(char.IsUpper))
            return false;

        if (!password.Any(char.IsLower))
            return false;

        if (!password.Any(char.IsDigit))
            return false;

        if (!password.Any(ch => "!@#$%^&*()_+-=[]{}|;:',.<>?/`~".Contains(ch)))
            return false;

        return true;
    }
}