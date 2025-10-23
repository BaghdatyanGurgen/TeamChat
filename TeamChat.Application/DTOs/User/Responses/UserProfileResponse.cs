
namespace TeamChat.Application.DTOs.User.Responses
{
    public class UserProfileResponse
    {
        public Guid UserId { get; internal set; }
        public string Email { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public string? AvatarUrl { get; internal set; }
        public DateTime CreatedAt { get; internal set; }
    }
}