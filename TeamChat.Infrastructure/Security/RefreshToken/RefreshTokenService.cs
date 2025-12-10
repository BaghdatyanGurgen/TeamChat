using System.Text;
using TeamChat.Domain.Entities;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Security;
using TeamChat.Infrastructure.Persistance;

namespace TeamChat.Infrastructure.Security.RefreshToken;

public class RefreshTokenService(AppDbContext db) : IRefreshTokenService
{
    private readonly AppDbContext _db = db;

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

        var userExists = await _db.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            throw new InvalidOperationException($"User with ID {userId} does not exist.");

        var refreshToken = new UserRefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = hash,
            PlainToken = newToken,
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
    public async Task<Guid> ValidateAsync(string token, string refreshToken)
    {
        var existingToken = await GetValidTokenAsync(refreshToken);
        if (existingToken == null || existingToken.UserId.ToString() != token)
            return Guid.Empty;

        return existingToken.UserId;
    }

    public async Task RevokeAsync(Guid userId)
    {
        var tokens = await _db.UserRefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAt == null)
            .ToListAsync();

        foreach (var t in tokens)
            t.RevokedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }


    private static string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    private static string ComputeHash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}

