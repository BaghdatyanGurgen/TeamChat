using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamChat.Infrastructure.Persistance.Configurations;

public class ChatRoleConfiguration : IEntityTypeConfiguration<ChatRole>
{
    public void Configure(EntityTypeBuilder<ChatRole> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).IsRequired();

        builder.HasOne(r => r.Chat)
               .WithMany(c => c.Roles)
               .HasForeignKey(r => r.ChatId);

        builder.HasMany(r => r.MemberRoles)
               .WithOne(mr => mr.ChatRole)
               .HasForeignKey(mr => mr.ChatRoleId);

        builder.HasIndex(r => new { r.ChatId, r.Name }).IsUnique();
    }
}