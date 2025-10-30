using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface ICompanyUserRepository
{
    Task<CompanyUser> Add(CompanyUser model);
}