using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Security;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
