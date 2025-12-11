using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamChat.Infrastructure.Persistance.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Content).IsRequired();

        builder.HasOne(m => m.Chat)
               .WithMany(c => c.Messages)
               .HasForeignKey(m => m.ChatId);

        builder.HasOne(m => m.Sender)
               .WithMany()
               .HasForeignKey(m => m.SenderId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.Attachments)
               .WithOne(a => a.Message)
               .HasForeignKey(a => a.MessageId);

        builder.HasMany(m => m.ReadStatuses)
               .WithOne(rs => rs.Message)
               .HasForeignKey(rs => rs.MessageId);
    }
}