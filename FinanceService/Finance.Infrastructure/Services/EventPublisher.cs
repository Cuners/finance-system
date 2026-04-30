using Finance.Application.DTO;
using Finance.Application.Services;
using Finance.Domain;
using MassTransit;
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
      //  private readonly IConnection _connection;
        private readonly ILogger<EventPublisher> _logger;
     //   private const string EXCHANGE_NAME = "NotificationService.Application.DTO.Events:TransactionCreatedEvent";
        private readonly IPublishEndpoint _publishEndpoint;
        public EventPublisher(IPublishEndpoint publishEndpoint, ILogger<EventPublisher> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }
        //MassTransit
        public async Task PublishTransactionCreatedAsync(int userId,
                                                          string email,
                                                          string accountName,
                                                          decimal balance,
                                                          decimal spentAmount,
                                                          int transactionId,
                                                          CancellationToken ct = default)
        {
            try
            {
                await _publishEndpoint.Publish(new TransactionCreatedEvent(
                    userId,
                    email,
                    accountName,
                    balance,
                    spentAmount), 
                    ct);

                _logger.LogInformation($"Published TransactionCreated event for TransactionId: {transactionId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to publish TransactionCreated event for TransactionId: {transactionId}");
            }
        }
        //Без MassTransit
        //public async Task PublishTransactionCreatedAsync(int userId, string email, string accountName, decimal balance, decimal spentAmount, int transactionId, CancellationToken ct = default)
        //{

        //    try
        //    {
        //        await using var channel = await _connection.CreateChannelAsync(null, ct);

        //        await channel.ExchangeDeclareAsync(
        //            exchange: EXCHANGE_NAME,
        //            type: ExchangeType.Fanout,
        //            durable: true,
        //            cancellationToken: ct);

        //        var @event = new TransactionCreatedEvent(
        //            userId,
        //            email,
        //            accountName,
        //            balance,
        //            spentAmount
        //           );

        //        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
        //        var properties = new BasicProperties()
        //        {
        //            Persistent = true,
        //            DeliveryMode = DeliveryModes.Persistent
        //        };

        //        await channel.BasicPublishAsync(
        //            exchange: EXCHANGE_NAME,
        //            routingKey: "",
        //            mandatory:false,
        //            basicProperties: properties,
        //            body: body,
        //            cancellationToken: ct);

        //        _logger.LogInformation($"Published TransactionCreated event for TransactionId: {transactionId}");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Failed to publish TransactionCreated event for TransactionId: {transactionId}");
        //    }
        //}
    }
}
