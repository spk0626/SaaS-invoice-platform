using Invoice.Application.Common.Messaging;
using Invoice.Application.Common.Interfaces;
using Invoice.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Invoice.Application.Invoices.EventHandlers;

public sealed class InvoiceSentDomainEventHandler 
    : INotificationHandler<DomainEventNotification<InvoiceSentDomainEvent>>
{
    private readonly IEmailService _email;
    private readonly ILogger<InvoiceSentDomainEventHandler> _logger;

    public InvoiceSentDomainEventHandler(
        IEmailService email,
        ILogger<InvoiceSentDomainEventHandler> logger)

    {
        _email = email;
        _logger = logger;
    }

    public async Task Handle(
        DomainEventNotification<InvoiceSentDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "Sending invoice {InvoiceNumber} to {Email}",
            domainEvent.InvoiceNumber,
            domainEvent.CustomerEmail);

        // Simulate sending an email notification to the customer
        await _email.SendInvoiceAsync(
            domainEvent.CustomerEmail,
            domainEvent.CustomerName,
            domainEvent.InvoiceNumber,
            domainEvent.Total,
            cancellationToken);

    }
    // inputs: 
    // DomainEventNotification<InvoiceSentDomainEvent> notification
    // CancellationToken cancellationToken

    // process: 
    // It extracts the InvoiceSentDomainEvent from the notification
    // logs the event
    // then simulates sending an email notification to the customer using the IEmailService.
    
    // outputs: none (void method, but it performs side effects by sending an email and

}