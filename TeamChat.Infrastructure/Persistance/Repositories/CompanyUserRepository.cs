using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Infrastructure.Persistance.Repositories.Base;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Infrastructure.Persistance.Repositories;

internal class CompanyUserRepository(AppDbContext context) 
    : BasicRepository<CompanyUser, Guid>(context), ICompanyUserRepository
{
    public Task<CompanyUser?> GetByUserAndCompany(Guid userId, int companyId)
    {
        var result = _dbSet
            .Include(cu => cu.User)
            .Include(cu => cu.Company)
            .Include(cu => cu.Position)
            .FirstOrDefaultAsync(cu => cu.UserId == userId && cu.CompanyId == companyId);

        return result;
    }
}