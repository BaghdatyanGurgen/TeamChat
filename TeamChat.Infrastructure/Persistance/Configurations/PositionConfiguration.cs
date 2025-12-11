using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamChat.Infrastructure.Persistance.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Title).IsRequired();
        builder.Property(p => p.InviteCode).IsRequired();

        builder.HasOne(p => p.Company)
               .WithMany()
               .HasForeignKey(p => p.CompanyId);

        builder.HasOne(p => p.ParentPosition)
               .WithMany(p => p.SubPositions)
               .HasForeignKey(p => p.ParentPositionId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}