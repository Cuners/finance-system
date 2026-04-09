using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationService.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(
            IOptions<EmailSettings> settings,
            ILogger<EmailSender> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendTransactionNotificationAsync(int userId,
                                                           string email,
                                                           string accountName,
                                                           decimal balance,
                                                           decimal spentAmount, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning($"Email not found for user {userId}");
                return;
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Новая транзакция";
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = BuildTransactionHtml(balance, accountName),
                TextBody = $"Кошелёк {accountName} ушёл в минус. Баланс: {balance:N2} ₽"
            };
            message.Body = bodyBuilder.ToMessageBody();
            await SendEmailAsync(message, ct);
        }

        public async Task SendBudgetExceededNotificationAsync(int userId, 
                                                              string email, 
                                                              string categoryName, 
                                                              decimal percentSpent, 
                                                              CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning($"Email not found for user {userId}");
                return;
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Превышен бюджет";
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = BuildBudgetHtml(categoryName, percentSpent),
                TextBody = $"Бюджет категории {categoryName} превышен на {percentSpent:N1}%"
            };
            message.Body = bodyBuilder.ToMessageBody();
            await SendEmailAsync(message, ct);
        }

        public async Task SendWelcomeEmailAsync(int userId, 
                                                string email, 
                                                string login, 
                                                CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning($"Email not found for user {userId}");
                return;
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Добро пожаловать в Budget App!";
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = BuildWelcomeHtml(login),
                TextBody = $"Здравствуйте, {login}! Добро пожаловать в Budget App."
            };
            message.Body = bodyBuilder.ToMessageBody();
            await SendEmailAsync(message, ct);
        }

        private async Task SendEmailAsync(MimeMessage message, CancellationToken ct)
        {
            try
            {
                using var client = new SmtpClient();
                // Для MailHog отключаем SSL
                SecureSocketOptions secureSocketOptions = SecureSocketOptions.None;
                if (_settings.SmtpUseSsl)
                {
                    secureSocketOptions = SecureSocketOptions.StartTls;
                }
                await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, secureSocketOptions, ct);
                if (!string.IsNullOrEmpty(_settings.SmtpUser))
                {
                    await client.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPassword, ct);
                }
                await client.SendAsync(message, ct);
                await client.DisconnectAsync(true, ct);
                _logger.LogInformation($"Email sent to {message.To}, subject: {message.Subject}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {message.To}");
                throw;
            }
        }

        private string BuildTransactionHtml(decimal balance, string accountName)
        {
            return $@"
            <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2 style='color: #ef4444;'>⚠️ Превышен бюджет кошелька</h2>
                    <p>Кошелёк: <strong>{accountName}</strong></p>
                    <p>Текущий баланс: <strong>{balance:N2} ₽</strong></p>
                    <p style='color: #ef4444;'>⚠️ Баланс отрицательный!</p>
                    <p>Рекомендуем пополнить кошелёк</p>
                    <hr style='border: 1px solid #e5e7eb;'/>
                    <p style='color: #6b7280; font-size: 12px;'>Budget App</p>
                </body>
            </html>";
        }

        private string BuildBudgetHtml(string categoryName, decimal percentSpent)
        {
            return $@"
            <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2 style='color: #ef4444;'>Превышен бюджет</h2>
                    <p>Категория: <strong>{categoryName}</strong></p>
                    <p>Использовано: <strong>{percentSpent:N1}%</strong></p>
                    <hr/>
                    <p style='color: #6b7280; font-size: 12px;'>Budget App</p>
                </body>
            </html>";
        }

        private string BuildWelcomeHtml(string login)
        {
            return $@"
            <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2 style='color: #0ea5e9;'>Добро пожаловать, {login}!</h2>
                    <p>Спасибо за регистрацию в Budget App.</p>
                    <hr/>
                    <p style='color: #6b7280; font-size: 12px;'>Budget App</p>
                </body>
            </html>";
        }
    }
}

