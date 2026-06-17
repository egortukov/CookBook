using CookBook.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookBook.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(r => r.Login)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(r => r.PasswordHash)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.HasIndex(u => u.Login).IsUnique();
    }
}