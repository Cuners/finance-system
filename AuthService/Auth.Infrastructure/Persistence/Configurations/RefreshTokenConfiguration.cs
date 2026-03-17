using Auth.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Jti)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.TokenHash)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(e => e.IsRevoked)
                .HasDefaultValue(false);

            builder.HasIndex(e => e.Jti).IsUnique();
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.IsRevoked);

            builder.HasOne(e => e.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_RefreshTokens_Users")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
