using Invoice.Application.Common.Interfaces;
using Invoice.Application.Common.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Invoice.Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IOptions<EmailSettings> settings,
        ILogger<EmailService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SendInvoiceAsync(
        string toEmail,
        string customerName,
        string invoiceNumber,
        decimal total,
        CancellationToken ct = default)
    {
        var subject = $"Invoice {invoiceNumber} from {_settings.FromName}";
        var body = $"""
            Dear {customerName},

            Please find your invoice {invoiceNumber} attached.

            Total amount: {total:C}

            Thank you for your business.

            {_settings.FromName}
            """;

        await SendAsync(toEmail, customerName, subject, body, ct);
    }

    public async Task SendInvoiceReminderAsync(
        string toEmail,
        string customerName,
        string invoiceNumber,
        decimal amountDue,
        DateTime dueDate,
        CancellationToken ct = default)
    {
        var subject = $"Reminder: Invoice {invoiceNumber} is overdue";
        var body = $"""
            Dear {customerName},

            This is a reminder that invoice {invoiceNumber} is overdue.

            Amount due: {amountDue:C}
            Original due date: {dueDate:dd MMMM yyyy}

            Please arrange payment at your earliest convenience.

            {_settings.FromName}
            """;

        await SendAsync(toEmail, customerName, subject, body, ct);
    }

    private async Task SendAsync(
        string toEmail,
        string toName,
        string subject,
        string body,
        CancellationToken ct)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromAddress));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(
                _settings.SmtpHost,
                _settings.SmtpPort,
                SecureSocketOptions.StartTls,
                ct);

            await client.AuthenticateAsync(
                _settings.Username, _settings.Password, ct);

            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);

            _logger.LogInformation(
                "Email sent to {Email}: {Subject}", toEmail, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to send email to {Email}: {Subject}", toEmail, subject);
            throw;
        }
    }
}