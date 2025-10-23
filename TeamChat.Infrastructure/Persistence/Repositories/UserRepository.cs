using TeamChat.Domain.Entities;
using TeamChat.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using TeamChat.Domain.Models.Exceptions.User;

namespace TeamChat.Infrastructure.Persistence.Repositories
{
    public class UserRepository(AppDbContext db) : IUserRepository
    {
        private readonly AppDbContext _db = db;

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _db.Users.FirstOrDefaultAsync(x => x.Email == email);

        public async Task AddAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> IsEmailAvailableAsync(string email) =>
            !await _db.Users.AnyAsync(x => x.Email == email);

        public async Task<Guid> SetPassword(Guid userId, string password)
        {
            var user = await GetByIdAsync(userId) ?? throw new UserNotFoundException();

            user.PasswordHash = HashPassword(password);

            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return user.Id;
        }




        private static string HashPassword(string password)
            => BCrypt.Net.BCrypt.HashPassword(password);
    }
}
