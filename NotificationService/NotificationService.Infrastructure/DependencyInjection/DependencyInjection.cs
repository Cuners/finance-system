using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Interfaces;
using NotificationService.Infrastructure.Persistence;
using NotificationService.Infrastructure.Persistence.Repositories;
using NotificationService.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<NotificationDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
            services.AddScoped<IWebSocketSender, WebSocketSender>();
            services.AddScoped<IEmailSender,EmailSender>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            return services;
        }
    }
}
