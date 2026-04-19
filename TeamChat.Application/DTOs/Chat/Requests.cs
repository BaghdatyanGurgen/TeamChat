using TeamChat.Domain.Enums;

namespace TeamChat.Application.DTOs.Chat;

public record CreateChatRequest(
    string Name,
    ChatScope Scope,
    int? DepartmentId,
    int? TeamId,
    int CompanyId,
    List<int>? PositionIds = null
);

public record CreatePrivateChatRequest(
    Guid TargetUserId,
    int CompanyId
);

public record AttachPositionToChatRequest(
    int PositionId,
    int? PermissionOverride = null
);

public record DetachPositionFromChatRequest(
    int PositionId
);