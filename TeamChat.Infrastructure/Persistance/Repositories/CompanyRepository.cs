using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class CompanyRepository(AppDbContext context) : ICompanyRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Company?> GetByIdAsync(int id)
        => await _context.Companies
            .Include(c => c.Chats)
            .Include(c => c.Members)
            .Include(c => c.Director)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<Company>> GetAllAsync()
        => await _context.Companies
            .Include(c => c.Chats)
            .Include(c => c.Members)
            .Include(c => c.Director)
            .ToListAsync();

    public async Task<Company> AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await SaveChangesAsync();
        return company;
    }
    public async Task Update(Company company)
    {
        _context.Companies.Update(company);
        await SaveChangesAsync();
    }
    public async Task Remove(Company company)
    {
        _context.Companies.Remove(company);
        await SaveChangesAsync();
    }
    public async Task<bool> ExistsAsync(int id)
        => await _context.Companies.AnyAsync(c => c.Id == id);
    
    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

}