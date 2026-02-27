using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Finance.Domain;
using Finance.Application.UseCases;

namespace Finance.Infrastructure.Persistence
{
    public partial class BudgetDbContext : DbContext, IUnitOfWork
    {
        public BudgetDbContext()
        {
        }

        public BudgetDbContext(DbContextOptions<BudgetDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Budget> Budgets { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Transaction> Transactions { get; set; }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await base.SaveChangesAsync(ct);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetDbContext).Assembly);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
