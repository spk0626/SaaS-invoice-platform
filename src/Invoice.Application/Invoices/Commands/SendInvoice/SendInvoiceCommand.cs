using Invoice.Domain.Exceptions;
using Invoice.Domain.Interfaces;
using MediatR;

namespace Invoice.Application.Invoices.Commands.SendInvoice;

public sealed record SendInvoiceCommand(Guid InvoiceId) : IRequest;

public sealed class SendInvoiceCommandHandler
:IRequestHandler<SendInvoiceCommand>
{
    private readonly IInvoiceRepository _invoices;

    public SendInvoiceCommandHandler(IInvoiceRepository invoices) =>
        _invoices = invoices;

    public async Task Handle(
        SendInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var invoice = await _invoices.GetByIdAsync(   // get invoice from database using provided ID
            request.InvoiceId, cancellationToken)
            ?? throw new NotFoundException("Invoice" , request.InvoiceId);

        invoice.Send();  //  marks the Status of the invoice as Sent to maybe perform any related business logic (e.g., sending email notification, updating status, etc.) -  By calling Send method of Invoice entity

        await _invoices.UpdateAsync(invoice, cancellationToken);  // updating the invoice in the database to reflect the changes made by the Send method

    }
}

