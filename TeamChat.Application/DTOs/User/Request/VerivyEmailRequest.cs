namespace TeamChat.Application.DTOs.User.Request
{
    public class VerivyEmailRequest
    {
        public Guid UserId { get; set; }
        public required string Token { get; set; }
    }
}