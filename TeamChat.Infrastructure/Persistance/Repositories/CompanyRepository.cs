using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Infrastructure.Persistance.Repositories.Base;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class CompanyRepository : BasicRepository<Company, int>, ICompanyRepository
{
    public CompanyRepository(AppDbContext context) : base(context) { }

    public override async Task<Company?> GetByIdAsync(int id)
    {
        return await _context.Companies
            .Include(c => c.Chats)
            .Include(c => c.Members)
            .Include(c => c.Director)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<CompanyUser>> GetEmployeesAsync(int companyId)
    {
        return await _context.CompanyUsers
            .Where(cu => cu.CompanyId == companyId)
            .ToListAsync();
    }

}
