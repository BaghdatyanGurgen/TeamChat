using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamChat.Infrastructure.Persistance.Configurations
{
    public class ChatMemberRoleConfiguration : IEntityTypeConfiguration<ChatMemberRole>
    {
        public void Configure(EntityTypeBuilder<ChatMemberRole> builder)
        {
            builder.HasKey(cm => cm.Id);
            builder.HasIndex(cm => new { cm.ChatMemberId, cm.ChatRoleId }).IsUnique();

            builder.HasOne(cm => cm.ChatMember)
                   .WithMany(c => c.Roles)
                   .HasForeignKey(cm => cm.ChatMemberId);

            builder.HasOne(cm => cm.ChatRole)
                   .WithMany(r => r.MemberRoles)
                   .HasForeignKey(cm => cm.ChatRoleId);
        }
    }
}