
namespace TeamChat.Application.DTOs.User.Responses
{
    public class RegisterEmailResponse
    {
        public Guid UserId { get; internal set; }
        public string Email { get; internal set; }
    }
}