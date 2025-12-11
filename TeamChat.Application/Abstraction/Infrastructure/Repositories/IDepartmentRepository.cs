using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;
using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IDepartmentRepository : IBasicRepository<Department, int>
{
    Task<IEnumerable<DepartmentMember>> GetEmployeesAsync(int id);
}