using TeamChat.Domain.Enums;
using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Infrastructure.Persistance.Repositories.Base;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

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
}