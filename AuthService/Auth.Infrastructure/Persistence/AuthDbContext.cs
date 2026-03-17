using Auth.Application.UseCases;
using Auth.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Infrastructure.Persistence
{
    public partial class AuthDbContext : DbContext, IUnitOfWork
    {
        public AuthDbContext()
        {

        }

        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Permission> Permissions { get; set; }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public async Task SaveChangesAsync(CancellationToken ct=default)
        {
            await base.SaveChangesAsync(ct);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
