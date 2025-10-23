using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using TeamChat.Infrastructure.Persistence;
using TeamChat.Domain.Entities;

namespace TeamChat.Infrastructure.Security.RefreshToken
{

    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext _db;

        public RefreshTokenService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<UserRefreshToken?> GetValidTokenAsync(string refreshToken)
        {
            var hash = ComputeHash(refreshToken);

            var token = await _db.UserRefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t =>
                    t.TokenHash == hash &&
                    t.RevokedAt == null &&
                    t.ExpiresAt > DateTime.UtcNow);

            return token;
        }

        public async Task<UserRefreshToken> CreateAsync(Guid userId)
        {
            var newToken = GenerateRefreshToken();
            var hash = ComputeHash(newToken);

            var refreshToken = new UserRefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TokenHash = hash,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            _db.UserRefreshTokens.Add(refreshToken);
            await _db.SaveChangesAsync();

            refreshToken.PlainToken = newToken;
            return refreshToken;
        }

        public async Task<bool> InvalidateAsync(UserRefreshToken token)
        {
            token.RevokedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        private string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        private string ComputeHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}

