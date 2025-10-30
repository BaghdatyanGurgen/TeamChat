using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamChat.Infrastructure.Persistance.Configurations
{
    public class ChatMemberConfiguration : IEntityTypeConfiguration<ChatMember>
    {
        public void Configure(EntityTypeBuilder<ChatMember> builder)
        {
            builder.HasKey(cm => cm.Id);
            builder.HasIndex(cm => new { cm.ChatId, cm.UserId }).IsUnique();

            builder.HasOne(cm => cm.Chat)
                   .WithMany(c => c.Members)
                   .HasForeignKey(cm => cm.ChatId);

            builder.HasOne(cm => cm.User)
                   .WithMany(u => u.ChatMemberships)
                   .HasForeignKey(cm => cm.UserId);

            builder.HasMany(cm => cm.Roles)
                   .WithOne(r => r.ChatMember)
                   .HasForeignKey(r => r.ChatMemberId);
        }
    }
}