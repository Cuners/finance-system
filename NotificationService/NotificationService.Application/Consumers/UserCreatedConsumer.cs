using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.Application.DTO.Events;
using NotificationService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Application.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly IUserEmailService _emailService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<UserCreatedConsumer> _logger;

        public UserCreatedConsumer(
            IUserEmailService emailService,
            IEmailSender emailSender,
            ILogger<UserCreatedConsumer> logger)
        {
            _emailService = emailService;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var @event = context.Message;

            _logger.LogInformation("Processing user created event for user {UserId}", @event.UserId);

            try
            {
                // Отправляем приветственное письмо
                await _emailSender.SendWelcomeEmailAsync(
                    @event.UserId,
                    @event.Login,
                    context.CancellationToken);

                _logger.LogInformation("Welcome email sent to user {UserId}", @event.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send welcome email to user {UserId}", @event.UserId);
            }
        }
    }
}
