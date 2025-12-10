namespace TeamChat.Domain.Enums
{
    [Flags]
    public enum PositionPermissions
    {
        None = 0,
        CreateDepartment = 1 << 0,
        CreatePosition = 1 << 1,
        CreateChat = 1 << 2,
        All = int.MaxValue
    }

}