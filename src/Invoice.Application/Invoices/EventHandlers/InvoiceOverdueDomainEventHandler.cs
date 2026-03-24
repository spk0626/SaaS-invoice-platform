using Invoice.Application.Common.Interfaces;
using Invoice.Application.Common.Messaging;
using Invoice.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Invoice.Application.Invoices.EventHandlers;

public sealed class InvoiceOverdueDomainHandler
: INotificationHandler<DomainEventNotification<InvoiceOverdueDomainEvent>>
{
    private readonly IEmailService _email;
    private readonly ILogger<InvoiceOverdueDomainHandler> _logger;

    public InvoiceOverdueDomainHandler(
        IEmailService email,
        ILogger<InvoiceOverdueDomainHandler> logger)

    {
        _email = email;
        _logger = logger;
    }

    public async Task Handle(
        DomainEventNotification<InvoiceOverdueDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "Invoice {InvoiceNumber} is overdue. Sending reminder to {Email}",
            domainEvent.InvoiceNumber,
            domainEvent.CustomerEmail);

        // Simulate sending an email reminder in Email Service class
        await _email.SendInvoiceReminderAsync(
            domainEvent.CustomerEmail,
            domainEvent.CustomerName,
            domainEvent.InvoiceNumber,
            domainEvent.AmountDue,
            domainEvent.DueDate,
            cancellationToken);

    }

}