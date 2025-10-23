using TeamChat.Domain.Entities;

namespace TeamChat.Infrastructure.Security.RefreshToken
{
    public interface IRefreshTokenService
    {
        Task<UserRefreshToken?> GetValidTokenAsync(string refreshToken);
        Task<UserRefreshToken> CreateAsync(Guid userId);
        Task<bool> InvalidateAsync(UserRefreshToken token);
    }
}