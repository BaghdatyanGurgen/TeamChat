using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;
using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface ICompanyRepository : IBasicRepository<Company, int>
{
    Task<IEnumerable<CompanyUser>> GetEmployeesAsync(int companyId);
}