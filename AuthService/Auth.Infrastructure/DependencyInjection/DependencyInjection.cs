using Auth.Application.Services;
using Auth.Domain.Interfaces;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Persistence.Repositories;
using Auth.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration config)
        {
            services.AddDbContext<AuthDbContext>(options =>
               options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
               sqlOptions => sqlOptions.EnableRetryOnFailure()));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPermissionRepository,PermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRefreshTokenRepository , RefreshTokenRepository>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITokenService, TokenService>();
            return services;

        }
    }
}
