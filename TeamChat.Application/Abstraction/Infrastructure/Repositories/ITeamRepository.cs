using TeamChat.Domain.Entities;
using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface ITeamRepository : IBasicRepository<Team, int>
{
    Task<IEnumerable<Team>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<TeamMember>> GetEmployeesAsync(int id);
}