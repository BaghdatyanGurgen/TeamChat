using TeamChat.Domain.Entities;
using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface ICompanyRepository : IBasicRepository<Company, int>
{
    Task<IEnumerable<CompanyUser>> GetEmployeesAsync(int companyId);
}