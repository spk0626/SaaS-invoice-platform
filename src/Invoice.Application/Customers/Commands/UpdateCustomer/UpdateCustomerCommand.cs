using Invoice.Domain.Entities;
using Invoice.Domain.Exceptions;
using Invoice.Domain.Interfaces;
using MediatR;

namespace Invoice.Application.Customers.Commands.UpdateCustomer;

public sealed record UpdateCustomerCommand(
    Guid Id,
    string Name,
    string? Phone,
    string? Address
): IRequest;  

public sealed class UpdateCustomerCommandHandler
: IRequestHandler<UpdateCustomerCommand>
{
    private readonly ICustomerRepository _customers;

    public UpdateCustomerCommandHandler(ICustomerRepository customers) =>
        _customers = customers;

    public async Task Handle(
        UpdateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _customers.GetByIdAsync(request.Id, cancellationToken)
           ?? throw new NotFoundException(nameof(Customer), request.Id);

        customer.Update( request.Name, request.Phone, request.Address );

        await _customers.UpdateAsync(customer, cancellationToken);
    }
    // inputs:
    // UpdateCustomerCommand request - a command object that contains the necessary information to update an existing customer, such as the customer's ID, name, email, phone, and address.
    // CancellationToken cancellationToken - a cancellation token that can be used to cancel the operation if needed.

    // process:
    // 1. The method first retrieves the existing customer from the database using the GetByIdAsync method of the ICustomerRepository, passing the customer's ID from the request. If the customer is not found, it throws a NotFoundException with the appropriate message.
    // 2. If the customer is found, it calls the Update method on the customer entity, passing the new name, phone, and address from the request to update the customer's information.
    // 3. Finally, it calls the UpdateAsync method of the ICustomerRepository to save the updated customer information back to the database.

    // outputs:
    // The method does not return any value (void), but it updates the existing customer's information in the database. If the customer with the specified ID is not found, it throws a NotFoundException instead of performing any update.
}