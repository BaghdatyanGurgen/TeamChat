using TeamChat.Domain.Entities;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using TeamChat.Domain.Models.Exceptions.User;
using TeamChat.Domain.Models.Exceptions.Company;

namespace TeamChat.Infrastructure.Persistance.Repositories;

internal class CompanyUserRepository(AppDbContext context) : ICompanyUserRepository
{
    private readonly AppDbContext _context = context;
    public async Task<CompanyUser> Add(CompanyUser companyUserModel)
    {
        var company = _context.Companies.FirstOrDefaultAsync(company => company.Id == companyUserModel.CompanyId) ?? throw new UserNotFoundException();
        var user = _context.Users.FirstOrDefaultAsync(user => user.Id == companyUserModel.UserId) ?? throw new CompanyNotFoundException();

        await _context.CompanyUsers.AddAsync(companyUserModel);
        await _context.SaveChangesAsync();
        return companyUserModel;
    }
}