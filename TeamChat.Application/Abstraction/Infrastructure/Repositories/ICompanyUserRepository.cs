using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;
using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface ICompanyUserRepository : IBasicRepository<CompanyUser, Guid>
{
    Task<CompanyUser?> GetByUserAndCompany(Guid userId, int companyId);
}