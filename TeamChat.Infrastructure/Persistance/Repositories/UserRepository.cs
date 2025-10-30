using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Domain.Models.Exceptions.User;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;

    public async Task<User?> GetByIdAsync(Guid id) =>
        await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<User?> GetByIdAsync(string? id)
    {
        if (Guid.TryParse(id, out var guidId))
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == guidId);

        return null;
    }

    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsEmailAvailableAsync(string email) =>
        !await _context.Users.AnyAsync(x => x.Email == email);

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
        var companyUsers = await _context.CompanyUsers.FirstOrDefaultAsync(u => u.CompanyId == companyId &&
                                                                     u.UserId == userId) ?? throw new UserNotFoundException();

        if (!companyUsers.IsActive)
            return false;

        companyUsers.IsActive = false;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<User> GetByEmailAndPasswordAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email) ?? throw new UserNotFoundException();

        if (!VerifyPassword(password, user.PasswordHash))
            throw new InvalidPasswordException();

        return user;
    }

    private static string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    private static bool VerifyPassword(string password, string passwordHash)
        => BCrypt.Net.BCrypt.Verify(password, passwordHash);

}