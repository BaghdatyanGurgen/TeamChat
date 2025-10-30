using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(int id);
    Task<IEnumerable<Company>> GetAllAsync();
    Task<Company> AddAsync(Company company);
    Task Update(Company company);
    Task Remove(Company company);
    Task<bool> ExistsAsync(int id);
    Task SaveChangesAsync();
}