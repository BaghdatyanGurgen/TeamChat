using TeamChat.Domain.Entities;

namespace TeamChat.Infrastructure.Security.Jwt
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
