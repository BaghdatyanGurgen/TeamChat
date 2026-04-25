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
                .Include(dm => dm.CompanyUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetByCompanyAsync(int companyId)
        {
            return await _context.Departments
                .Where(d => d.CompanyId == companyId)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }
        public async Task<IEnumerable<DepartmentMember>> GetByCompanyUserAsync(int companyUserId)
        {
            return await _context.DepartmentMembers
                .Where(dm => dm.CompanyUserId == companyUserId)
                .ToListAsync();
        }
    }
}