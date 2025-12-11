using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamChat.Infrastructure.Persistance.Configurations;

public class MessageReadStatusConfiguration : IEntityTypeConfiguration<MessageReadStatus>
{
    public void Configure(EntityTypeBuilder<MessageReadStatus> builder)
    {
        builder.HasKey(rs => rs.Id);
        builder.HasIndex(rs => new { rs.MessageId, rs.UserId }).IsUnique();

        builder.HasOne(rs => rs.Message)
               .WithMany(m => m.ReadStatuses)
               .HasForeignKey(rs => rs.MessageId);

        builder.HasOne(rs => rs.User)
               .WithMany()
               .HasForeignKey(rs => rs.UserId);
    }
}