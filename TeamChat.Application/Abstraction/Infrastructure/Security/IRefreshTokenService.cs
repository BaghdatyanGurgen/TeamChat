using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Security;

public interface IRefreshTokenService
{
    Task<UserRefreshToken?> GetValidTokenAsync(string refreshToken);
    Task<UserRefreshToken> CreateAsync(Guid userId);
    Task<Guid> ValidateAsync(string token, string refreshToken);
    Task RevokeAsync(Guid userId);
}