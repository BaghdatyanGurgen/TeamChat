using TeamChat.Domain.Entities;

namespace TeamChat.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> IsEmailAvailableAsync(string email);
        Task<Guid> SetPassword(Guid userId, string password);
    }
}
    