using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Domain.Entities;
using TeamChat.Domain.Models.Exceptions.User;
using TeamChat.Infrastructure.Persistance.Repositories.Base;

namespace TeamChat.Infrastructure.Persistance.Repositories;
public class UserRepository : BasicRepository<User, Guid>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<User?> GetByIdAsync(string? id)
    {
        if (Guid.TryParse(id, out var guidId))
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == guidId);

        return null;
    }

    public async Task<User?> GetByEmailAsync(string email) =>
        await _dbSet.FirstOrDefaultAsync(x => x.Email == email);

    public async Task<bool> IsEmailAvailableAsync(string email) =>
        !await _dbSet.AnyAsync(x => x.Email == email);

    public async Task<User> SetPassword(Guid userId, string password)
    {
        var user = await GetByIdAsync(userId) ?? throw new UserNotFoundException();

        user.PasswordHash = HashPassword(password);

        if (string.IsNullOrEmpty(user.FirstName) && string.IsNullOrEmpty(user.LastName))
        {
            user.FirstName = "New";
            user.LastName = "User";
        }

        await UpdateAsync(user);

        return user;
    }

    public async Task<bool> DeactivateUserInCompanyAsync(Guid userId, int companyId)
    {
        var companyUser = await _context.CompanyUsers
            .FirstOrDefaultAsync(u => u.CompanyId == companyId && u.UserId == userId)
            ?? throw new UserNotFoundException();

        if (!companyUser.IsActive)
            return false;

        companyUser.IsActive = false;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<User> GetByEmailAndPasswordAsync(string email, string password)
    {
        var user = await _dbSet.FirstOrDefaultAsync(u => u.Email == email) ?? throw new UserNotFoundException();

        if (!VerifyPassword(password, user.PasswordHash))
            throw new InvalidPasswordException();

        return user;
    }

    private static string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    private static bool VerifyPassword(string password, string passwordHash)
        => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
