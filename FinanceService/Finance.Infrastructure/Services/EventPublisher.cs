using Finance.Application.DTO;
using Finance.Application.Services;
using Finance.Domain;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Finance.Infrastructure.Services
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IConnection _connection;
        private readonly ILogger<EventPublisher> _logger;
        private const string EXCHANGE_NAME = "notifications.events";

        public EventPublisher(IConnection connection, ILogger<EventPublisher> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task PublishBudgetExceededAsync(int userId, string accountName, string email, decimal balance, decimal spentAmount, int transactionId, CancellationToken ct = default)
        {

            try
            {
                await using var channel = await _connection.CreateChannelAsync(null, ct);

                await channel.ExchangeDeclareAsync(
                    exchange: EXCHANGE_NAME,
                    type: ExchangeType.Topic,
                    durable: true,
                    cancellationToken: ct);

                var @event = new BudgetExceededEvent(
                    userId,
                    email,
                    accountName,
                    balance,
                    spentAmount
                   );

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
                var properties = new BasicProperties()
                {
                    Persistent = true,
                    DeliveryMode = DeliveryModes.Persistent
                };

                await channel.BasicPublishAsync(
                    exchange: EXCHANGE_NAME,
                    routingKey: "budgetAccount.exceeded",
                    mandatory:false,
                    basicProperties: properties,
                    body: body,
                    cancellationToken: ct);

                _logger.LogInformation($"Published budgetAccount.exceeded event for TransactionId: {transactionId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to publish budgetAccount.exceeded event for TransactionId: {transactionId}");
            }
        }
    }
}
