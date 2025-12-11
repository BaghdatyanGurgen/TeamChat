using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamChat.Infrastructure.Persistance.Configurations;

public class MessageAttachmentConfiguration : IEntityTypeConfiguration<MessageAttachment>
{
    public void Configure(EntityTypeBuilder<MessageAttachment> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.FileUrl).IsRequired();

        builder.HasOne(a => a.Message)
               .WithMany(m => m.Attachments)
               .HasForeignKey(a => a.MessageId);
    }
}