using MassTransit;
using Microsoft.Extensions.Logging;
using NotificationService.Application.DTO.Events;
using NotificationService.Application.Interfaces;
using NotificationService.Domain;
using NotificationService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace NotificationService.Application.Handlers
{
    public class TransactionCreatedConsumer : IConsumer<TransactionCreatedEvent>
    {
        private readonly INotificationRepository _repo;
        private readonly IEmailSender _emailSender;
        private readonly IWebSocketSender _webSocketSender;
        private readonly ILogger<TransactionCreatedConsumer> _logger;

        public TransactionCreatedConsumer(
            INotificationRepository repo,
            IEmailSender emailSender,
            IWebSocketSender webSocketSender,
            ILogger<TransactionCreatedConsumer> logger)
        {
            _repo = repo;
            _emailSender = emailSender;
            _webSocketSender = webSocketSender;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<TransactionCreatedEvent> context)
        {
            var @event = context.Message;

            _logger.LogInformation($"Processing transaction created event for user {@event.UserId}");

            try
            {
                var notification = new Notification
                {
                    UserId = @event.UserId,
                    NotificationTypeId = 1,
                    Title = "Превышен бюджет кошелька",
                    Message = $"Кошелёк '{@event.AccountName}' ушёл в минус. Баланс: {@event.Balance:N2} ₽",
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false,
                    Data = JsonSerializer.Serialize(new { @event.UserId, @event.AccountName, @event.Balance, @event.SpentAmount })
                };

                await _repo.SaveAsync(notification, context.CancellationToken);
                //Отправляем WebSocket
                await _webSocketSender.SendToUserAsync(@event.UserId, new
                {
                    type = "budgetAccount.exceeded",
                    notificationId = notification.NotificationId,
                    data = new { @event.UserId, @event.AccountName, @event.Balance, @event.SpentAmount }
                }, context.CancellationToken);
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _emailSender.SendTransactionNotificationAsync(
                            @event.UserId,
                            @event.Email,
                            @event.AccountName, 
                            @event.Balance, 
                            @event.SpentAmount,
                            context.CancellationToken);

                        notification.EmailSentAt = DateTime.UtcNow;
                        await _repo.UpdateAsync(notification, context.CancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Failed to send email for transaction {@event.UserId}");
                    }
                }, context.CancellationToken);

                _logger.LogInformation($"Transaction notification processed successfully for user {@event.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to process transaction created event for user {@event.UserId}");
                throw; 
            }
        }
    }
}
