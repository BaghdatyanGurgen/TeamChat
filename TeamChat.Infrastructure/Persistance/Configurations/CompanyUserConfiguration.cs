using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamChat.Infrastructure.Persistance.Configurations
{
    public class CompanyUserConfiguration : IEntityTypeConfiguration<CompanyUser>
    {
        public void Configure(EntityTypeBuilder<CompanyUser> builder)
        {
            builder.HasKey(cu => cu.Id);

            builder.HasOne(cu => cu.User)
                   .WithMany()
                   .HasForeignKey(cu => cu.UserId);

            builder.HasOne(cu => cu.Company)
                   .WithMany()
                   .HasForeignKey(cu => cu.CompanyId);

            builder.HasOne(cu => cu.Position)
                   .WithMany()
                   .HasForeignKey(cu => cu.PositionId);

            builder.HasIndex(cu => new { cu.UserId, cu.CompanyId }).IsUnique();
        }
    }
}