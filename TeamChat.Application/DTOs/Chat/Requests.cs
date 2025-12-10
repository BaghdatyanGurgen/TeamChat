using TeamChat.Domain.Enums;

namespace TeamChat.Application.DTOs.Chat;

public record CreateChatRequest(string Name, ChatScope Scope, int? DepartmentId, int? TeamId, int CompanyId);