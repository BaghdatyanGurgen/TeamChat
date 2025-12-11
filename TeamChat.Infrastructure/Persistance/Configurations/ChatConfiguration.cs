using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamChat.Infrastructure.Persistance.Configurations;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired();

        builder.HasOne(c => c.Owner)
               .WithMany()
               .HasForeignKey(c => c.OwnerId);

        builder.HasOne(c => c.Company)
               .WithMany(c => c.Chats)
               .HasForeignKey(c => c.CompanyId);

        builder.HasMany(c => c.Members)
               .WithOne(cm => cm.Chat)
               .HasForeignKey(cm => cm.ChatId);

        builder.HasMany(c => c.Roles)
               .WithOne(r => r.Chat)
               .HasForeignKey(r => r.ChatId);

        builder.HasMany(c => c.Messages)
               .WithOne(m => m.Chat)
               .HasForeignKey(m => m.ChatId);
    }
}
