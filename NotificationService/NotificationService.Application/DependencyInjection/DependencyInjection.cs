using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Consumers;
using NotificationService.Application.DTO.Events;
using NotificationService.Application.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IConsumer<TransactionCreatedEvent>, TransactionCreatedConsumer>();
            services.AddScoped<IConsumer<UserCreatedEvent>, UserCreatedConsumer>();
            return services;
        }
    }
}
