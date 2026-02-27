using Finance.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(e => e.AccountId)
                .HasName("PK__Accounts__349DA5A6F08B61D8");

            builder.Property(e => e.Name)
                .HasMaxLength(100);

            builder.Property(e => e.Balance)
                .HasColumnType("decimal(14, 2)");
        }
    }
}
