using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Application.DTOs.Company;
using TeamChat.Domain.Entities;
using TeamChat.Domain.Enums;
using TeamChat.Infrastructure.Persistance.Repositories.Base;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class PositionRepository(AppDbContext context)
        : BasicRepository<Position, int>(context), IPositionRepository
{
    public async Task<bool> CanCreateChat(Guid ownerId, int companyId)
    {
        var companyUser = await _context.CompanyUsers
            .Include(cu => cu.Company)
            .FirstOrDefaultAsync(cu => cu.UserId == ownerId && cu.CompanyId == companyId);

        if (companyUser == null)
            return false;

        var position = await _context.Positions
            .FirstOrDefaultAsync(p => p.Id == companyUser.PositionId && p.CompanyId == companyId);

        if (position == null)
            return false;

        return CheckPermission(position, PositionPermissions.CreateChat);
    }

    private static bool CheckPermission(Position position, PositionPermissions permission)
        => (position.Permissions & permission) == permission;

    public async Task<Position?> GetByInviteCodeAsync(string inviteCode)
    {
        return await _dbSet.Include(p => p.Company)
            .FirstOrDefaultAsync(p => p.InviteCode == inviteCode);
    }

    public async Task<List<Position>> GetUserPositionsAsync(Guid userId, int companyId)
    {
        return await _dbSet
            .Where(p => p.CompanyId == companyId && p.CreatedByUserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }
}