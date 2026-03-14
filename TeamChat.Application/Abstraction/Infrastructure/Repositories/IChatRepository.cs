using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;
using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IChatRepository: IBasicRepository<Chat, Guid>
{
    Task<List<Chat>> GetByDepartment(int? departmentId);
    Task<List<Chat>> GetUserChatsAsync(Guid userId);
    Task<List<Chat>> GetUserCompanyChatsAsync(Guid userId, int companyId);
}