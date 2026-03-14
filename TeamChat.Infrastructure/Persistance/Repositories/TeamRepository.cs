using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Infrastructure.Persistance.Repositories.Base;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class TeamRepository(AppDbContext db)
        : BasicRepository<Team, int>(db), ITeamRepository
{
    public async Task<IEnumerable<Team>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Teams
            .Include(t => t.Members)
            .ThenInclude(t=> t.CompanyUser)
            .Where(t => t.Members.Any(tm => tm.CompanyUser.UserId == userId))
            .ToListAsync();
    }

    public async Task<IEnumerable<TeamMember>> GetEmployeesAsync(int id)
    {
        return await _context.TeamMembers
            .Where(tm=> tm.TeamId == id)
            .Include(tm=> tm.CompanyUser)
            .ToListAsync();
    }
}