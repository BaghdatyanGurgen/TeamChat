namespace TeamChat.Domain.Enums;

[Flags]
public enum PositionPermissions
{
    All = int.MaxValue,
    None = 0,
    CreateDepartment = 1 << 0,
    CreatePosition = 1 << 1,
    CreateChat = 1 << 2,
    AddChatMember = 1 << 3,
    RemoveChatMember = 1 << 4,
    EditMessage = 1 << 5,
    DeleteMessage = 1 << 6
}