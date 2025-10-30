namespace TeamChat.Domain.Enums
{
    [Flags]
    public enum PositionPermissions
    {
        None = 0,
        CreateDepartment = 1 << 0,
        CreatePosition = 1 << 1,
        EditPosition = 1 << 2,
        DeletePosition = 1 << 3,
        InviteUsers = 1 << 4,
        ApproveUsers = 1 << 5,
        RemoveUsers = 1 << 6,
        ChangeUserRole = 1 << 7,
        CreateChat = 1 << 8,
        ManageChat = 1 << 9,
        SendBroadcastMessage = 1 << 10,
        ViewReports = 1 << 11,
        ManageReports = 1 << 12,
        ManageCompany = 1 << 13,
        ManagePermissions = 1 << 14,
        All = int.MaxValue
    }

}