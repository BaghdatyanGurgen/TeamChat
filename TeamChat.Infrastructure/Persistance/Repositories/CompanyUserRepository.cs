using TeamChat.Domain.Entities;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using TeamChat.Domain.Models.Exceptions.User;
using TeamChat.Domain.Models.Exceptions.Company;
using TeamChat.Infrastructure.Persistance.Repositories.Base;

namespace TeamChat.Infrastructure.Persistance.Repositories;

internal class CompanyUserRepository : BasicRepository<CompanyUser, Guid>, ICompanyUserRepository
{
    public CompanyUserRepository(AppDbContext context) : base(context) { }

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
