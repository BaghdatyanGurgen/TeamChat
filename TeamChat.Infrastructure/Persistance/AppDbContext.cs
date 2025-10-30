        using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TeamChat.Infrastructure.Persistance;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<ChatMember> ChatMembers => Set<ChatMember>();
    public DbSet<ChatRole> ChatRoles => Set<ChatRole>();
    public DbSet<ChatMemberRole> ChatMemberRoles => Set<ChatMemberRole>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<MessageAttachment> MessageAttachments => Set<MessageAttachment>();
    public DbSet<MessageReadStatus> MessageReadStatuses => Set<MessageReadStatus>();
    public DbSet<UserRefreshToken> UserRefreshTokens => Set<UserRefreshToken>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<CompanyUser> CompanyUsers => Set<CompanyUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}