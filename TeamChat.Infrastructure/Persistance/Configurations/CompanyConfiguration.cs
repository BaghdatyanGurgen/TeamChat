using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamChat.Infrastructure.Persistance.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired();

            builder.HasOne(c => c.Director)
                   .WithMany(u => u.ManagedCompanies)
                   .HasForeignKey(c => c.DirectorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Chats)
                   .WithOne(ch => ch.Company)
                   .HasForeignKey(ch => ch.CompanyId);
        }
    }
}