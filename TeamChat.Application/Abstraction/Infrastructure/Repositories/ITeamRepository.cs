using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;
using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface ITeamRepository : IBasicRepository<Team, int>
{
    Task<IEnumerable<TeamMember>> GetEmployeesAsync(int id);
}