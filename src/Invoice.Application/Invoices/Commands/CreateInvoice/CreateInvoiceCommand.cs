using Invoice.Domain.Entities;
using Invoice.Domain.Interfaces;
using Invoice.Domain.Exceptions;
using MediatR;
using InvoiceEntity = Invoice.Domain.Entities.Invoice;

namespace Invoice.Application.Invoices.Commands.CreateInvoice;

public sealed record CreateInvoiceCommand(
    Guid CustomerId,
    DateTime DueDate,
    string Currency,
    decimal TaxRate,
    string? Notes,
    List<CreateInvoiceItemRequest> Items
): IRequest<Guid>;  
// data shape of a command to create a new invoice

public sealed record CreateInvoiceItemRequest(
    string Description,
    decimal Quantity,
    decimal UnitPrice
);
// data shape of an individual new invoice item, that will later be used for creating a new invoice

public sealed class CreateInvoiceCommandHandler
: IRequestHandler<CreateInvoiceCommand, Guid>
{
    private readonly IInvoiceRepository _invoices;
    private readonly ICustomerRepository _customers;

    public CreateInvoiceCommandHandler(
        IInvoiceRepository invoices,
        ICustomerRepository customers)
    {
        _invoices = invoices;
        _customers = customers;
    }

    public async Task<Guid> Handle(
        CreateInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _customers.GetByIdAsync(
            request.CustomerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), request.CustomerId);

        var invoice = InvoiceEntity.Create(    // creating new invoice in system
            request.CustomerId,
            request.DueDate,
            request.Currency,
            request.TaxRate,
            request.Notes); 

        foreach (var item in request.Items)            // add each item in Items list to the new invoice
            invoice.AddItem(item.Description, item.Quantity, item.UnitPrice);   // inputs to call the AddItem method of Invoice entity, which calls Create method of InvoiceItem entity inside it.

        await _invoices.AddAsync(invoice, cancellationToken);    // adding created new invoice to database

        return invoice.Id;
    }
    // inputs:
    // CreateInvoiceCommand request 
    // CancellationToken cancellationToken 

    // process:
    // 1. The method first attempts to retrieve the customer from the database using the GetByIdAsync method of the ICustomerRepository, passing the customer's ID from the request and the cancellation token. If the customer is not found, it throws a NotFoundException.
    // 2. If the customer is found, it creates a new Invoice entity using the Create factory method, passing the necessary information from the request such as CustomerId, DueDate, Currency, TaxRate, and Notes.
    // 3. Then, it iterates through the list of invoice items provided in the request and adds each item to the invoice using the AddItem method.
    // 4. After all items have been added, it adds the new invoice to the database by calling the AddAsync method of the IInvoiceRepository.
    // 5. Finally, it returns the ID of the newly created invoice.

    // outputs:
    // returns a Guid, which is the ID of the newly created invoice. 
}

// inputs of CreateInvoiceCommandHandler -
// CreateInvoiceCommand request

// process of CreateInvoiceCommandHandler -
// 1. Repository interface instances are created
// 2. Repository interfaces are injected into the constructor of the handler class
// 3. The handler method retrieves the customer from the database using the provided ID
// 4. If customer is found, a new invoice entity is created using the Create factory method, passing the necessary information from the request and stored in memory as an instance of the Invoice entity
// 5. Then, it iterates through the list of invoice items provided in the request and adds each item to the invoice using the AddItem method.
// 6. After all items have been added, it adds the new invoice to the database by calling the AddAsync method of the IInvoiceRepository.   
// 7. It returns the ID of the newly created invoice.

// outputs of CreateInvoiceCommandHandler -
// Guid, the ID of the new invoice

// The AddAsync method of the IInvoiceRepository is responsible for adding the invoice entity to the database context and saving the changes, which results in the new invoice being stored in the database and assigned a unique ID. The returned Guid is the ID of the newly created invoice, which can be used for further operations or references to that invoice.