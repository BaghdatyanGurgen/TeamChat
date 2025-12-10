using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Domain.Entities;
using TeamChat.Infrastructure.Persistance.Repositories.Base;

namespace TeamChat.Infrastructure.Persistance.Repositories
{
    public class DepartmentRepository : BasicRepository<Department, int>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DepartmentMember>> GetEmployeesAsync(int id)
        {
            return await _context.DepartmentMembers
                .Where(dm => dm.DepartmentId == id)
                .Include(dm=> dm.CompanyUser)
                .ToListAsync();
        }
    }
}
