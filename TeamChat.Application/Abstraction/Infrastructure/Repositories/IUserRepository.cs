using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByIdAsync(string? id);
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task<bool> IsEmailAvailableAsync(string email);
    Task<User> SetPassword(Guid userId, string password);
    Task<User> GetByEmailAndPasswordAsync(string email, string password);
    Task<bool> DeactivateUserInCompanyAsync(Guid userId, int companyId);
}