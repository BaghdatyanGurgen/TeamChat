using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamChat.Infrastructure.Persistance.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.FirstName).IsRequired();
        builder.Property(u => u.LastName).IsRequired();
        builder.Property(u => u.PasswordHash).IsRequired();

        builder.HasMany(u => u.ChatMemberships)
               .WithOne(cm => cm.User)
               .HasForeignKey(cm => cm.UserId);

        builder.HasMany(u => u.ManagedCompanies)
               .WithOne(c => c.Director)
               .HasForeignKey(c => c.DirectorId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}