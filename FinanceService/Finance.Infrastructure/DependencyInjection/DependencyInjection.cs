using Finance.Application.Services;
using Finance.Domain.Interfaces;
using Finance.Infrastructure.Persistence;
using Finance.Infrastructure.Persistence.Repositories;
using Finance.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<BudgetDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IBudgetRepository, BudgetRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddSingleton<ICacheService, CacheAsideService>();
            services.AddSingleton<ITransactionCacheInvalidator, TransactionCacheInvalidator>();
            services.AddSingleton<IAccountCacheInvalidator, AccountCacheInvalidator>();
            services.AddSingleton<IBudgetCacheInvalidator, BudgetCacheInvalidator>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            //services.AddScoped<IEventPublisher, EventPublisher>();
            return services;
        }
    }
}
