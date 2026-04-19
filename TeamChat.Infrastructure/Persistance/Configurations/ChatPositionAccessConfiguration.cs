using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamChat.Infrastructure.Persistance.Configurations;

public class ChatPositionAccessConfiguration
    : IEntityTypeConfiguration<ChatPositionAccess>
{
    public void Configure(EntityTypeBuilder<ChatPositionAccess> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.PositionId, x.ChatId }).IsUnique();

        builder.HasOne(x => x.Position)
            .WithMany(p => p.ChatAccess)
            .HasForeignKey(x => x.PositionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Chat)
            .WithMany(c => c.PositionAccess)
            .HasForeignKey(x => x.ChatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}