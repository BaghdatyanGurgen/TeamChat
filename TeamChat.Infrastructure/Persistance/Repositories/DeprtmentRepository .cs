using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Infrastructure.Persistance.Repositories.Base;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Infrastructure.Persistance.Repositories
{
    public class DepartmentRepository(AppDbContext context) 
        : BasicRepository<Department, int>(context), IDepartmentRepository
    {
        public async Task<IEnumerable<DepartmentMember>> GetEmployeesAsync(int id)
        {
            return await _context.DepartmentMembers
                .Where(dm => dm.DepartmentId == id)
                .Include(dm=> dm.CompanyUser)
                .ToListAsync();
        }
    }
}