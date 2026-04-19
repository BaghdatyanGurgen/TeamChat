using TeamChat.Domain.Entities;
using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IDepartmentRepository : IBasicRepository<Department, int>
{
    Task<IEnumerable<DepartmentMember>> GetEmployeesAsync(int id);
    Task<IEnumerable<Department>> GetByCompanyAsync(int companyId);
}