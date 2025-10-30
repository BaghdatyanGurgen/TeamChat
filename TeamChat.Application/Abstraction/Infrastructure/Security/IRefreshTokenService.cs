using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Security
{
    public interface IRefreshTokenService
    {
        Task<UserRefreshToken?> GetValidTokenAsync(string refreshToken);
        Task<UserRefreshToken> CreateAsync(Guid userId);
        Task<bool> InvalidateAsync(UserRefreshToken token);
    }
}