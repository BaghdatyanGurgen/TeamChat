using Microsoft.EntityFrameworkCore;
using TeamChat.Domain.Entities;

namespace TeamChat.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Chat> Chats => Set<Chat>();
        public DbSet<ChatMember> ChatMembers => Set<ChatMember>();
        public DbSet<ChatRole> ChatRoles => Set<ChatRole>();
        public DbSet<ChatMemberRole> ChatMemberRoles => Set<ChatMemberRole>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<MessageAttachment> MessageAttachments => Set<MessageAttachment>();
        public DbSet<MessageReadStatus> MessageReadStatuses => Set<MessageReadStatus>();
        public DbSet<UserRefreshToken> UserRefreshTokens => Set<UserRefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatMemberRole>()
                .HasIndex(x => new { x.ChatMemberId, x.ChatRoleId })
                .IsUnique();

            modelBuilder.Entity<ChatMember>()
                .HasIndex(x => new { x.ChatId, x.UserId })
                .IsUnique();

            modelBuilder.Entity<ChatMember>()
                .HasOne(cm => cm.Chat)
                .WithMany(c => c.Members)
                .HasForeignKey(cm => cm.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChatMember>()
                .HasOne(cm => cm.User)
                .WithMany(u => u.ChatMemberships)
                .HasForeignKey(cm => cm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChatRole>()
                .HasOne(r => r.Chat)
                .WithMany(c => c.Roles)
                .HasForeignKey(r => r.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MessageReadStatus>()
                .HasOne(r => r.Message)
                .WithMany(m => m.ReadStatuses)
                .HasForeignKey(r => r.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MessageReadStatus>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
