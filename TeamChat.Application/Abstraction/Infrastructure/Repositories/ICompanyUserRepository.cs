using TeamChat.Domain.Entities;
using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface ICompanyUserRepository : IBasicRepository<CompanyUser, Guid>
{
    Task<CompanyUser?> GetByUserAndCompany(Guid userId, int companyId);
}