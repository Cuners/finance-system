using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Auth.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Auth.Infrastructure.Persistence.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.Property(e => e.PermissionName).HasMaxLength(100);
        }
    }
}
