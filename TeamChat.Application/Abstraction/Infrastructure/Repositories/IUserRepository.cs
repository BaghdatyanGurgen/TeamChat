using TeamChat.Domain.Entities;
using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IUserRepository : IBasicRepository<User, Guid>
{
    Task<User?> GetByIdAsync(string? id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> IsEmailAvailableAsync(string email);
    Task<User> SetPassword(Guid userId, string password);
    Task<User> GetByEmailAndPasswordAsync(string email, string password);
    Task<bool> DeactivateUserInCompanyAsync(Guid userId, int companyId);
}