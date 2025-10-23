namespace TeamChat.Application.DTOs.User.Request
{
    public class SetPasswordRequest
    {
        public Guid UserId { get; set; }
        public required string Password { get; set; }
    }
}