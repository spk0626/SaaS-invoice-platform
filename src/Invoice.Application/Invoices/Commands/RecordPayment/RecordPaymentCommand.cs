using Invoice.Domain.Enums;
using Invoice.Domain.Exceptions;
using Invoice.Domain.Interfaces;
using MediatR;

namespace Invoice.Application.Invoices.Commands.RecordPayment;

public sealed record RecordPaymentCommand(
    Guid InvoiceId,
    decimal Amount,
    PaymentMethod Method,
    string? Reference
) : IRequest;


public sealed class RecordPaymentCommandHandler
: IRequestHandler<RecordPaymentCommand>
{
    private readonly IInvoiceRepository _invoices;

    public RecordPaymentCommandHandler(IInvoiceRepository invoices) =>
        _invoices = invoices;

    public async Task Handle(
        RecordPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var invoice = await _invoices.GetByIdAsync(   // get invoice from database using provided ID
            request.InvoiceId, cancellationToken)
            ?? throw new NotFoundException("Invoice", request.InvoiceId);

        invoice.RecordPayment(request.Amount, request.Method, request.Reference);  // creates a new Payment object with payment data and adds it to a list of payments of the invoice and changes the Status of the invoice to Paid - By calling RecordPayment method of Invoice entity, which calls Create method of Payment entity inside it.

        await _invoices.UpdateAsync(invoice, cancellationToken);  // updating the invoice in the database 

    }
}