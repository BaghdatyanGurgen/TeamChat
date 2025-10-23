
namespace TeamChat.Application.DTOs.User.Request
{
    public class UpdateUserProfileRequest
    {
        public string? FirstName { get; internal set; }
        public Guid UserId { get; internal set; }
        public string? LastName { get; internal set; }
        public string? AvatarUrl { get; internal set; }
    }
}