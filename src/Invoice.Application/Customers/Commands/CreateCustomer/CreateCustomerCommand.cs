using Invoice.Domain.Entities;
using Invoice.Domain.Exceptions;
using Invoice.Domain.Interfaces;
using MediatR;

namespace Invoice.Application.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(
    string Name,
    string Email,
    string? Phone,
    string? Address
): IRequest<Guid>;  // A record type that represents a command to create a new customer, containing the necessary information such as name, email, phone, and address. 
// It implements the IRequest interface from MediatR, indicating that it is a request that expects a response of type Guid (the ID of the newly created customer).

public sealed class CreateCustomerCommandHandler 
: IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _customers;  // A repository interface for accessing customer data, which will be used to check for existing customers and add new customers to the database.

    public CreateCustomerCommandHandler(ICustomerRepository customers) =>  
        _customers = customers;  
    // Constructor that takes an ICustomerRepository instance and assigns it to the private field

    public async Task<Guid> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var emailExists = await _customers.EmailExistsAsync(
            request.Email, cancellationToken);

        if (emailExists)
        throw new BusinessRuleException(
                $"A customer with email '{request.Email}' already exists.");

        var customer =  Customer.Create(
            request.Name,
            request.Email,
            request.Phone,
            request.Address
        );

        await _customers.AddAsync(customer, cancellationToken);
        return customer.Id;
    }
    // inputs:
    // CreateCustomerCommand request - a command object that contains the necessary information to create a new customer, such as name, email, phone, and address.
    // CancellationToken cancellationToken - a cancellation token that can be used to cancel the operation if needed.

    // process: 
    // 1. The method first checks if a customer with the same email already exists in the database by calling the EmailExistsAsync method of the ICustomerRepository. 
    // 2. If a customer with the same email exists, it throws a BusinessRuleException with an appropriate message. 
    // 3. If the email is unique, it creates a new Customer entity using the Create factory method, passing the necessary information from the request. 
    // 4. Then, it adds the new customer to the database by calling the AddAsync method of the ICustomerRepository. 
    // 5. Finally, it returns the ID of the newly created customer.

    // outputs:
    // customer.id - The method returns a Guid, which is the ID of the newly created customer. If a customer with the same email already exists, it throws a BusinessRuleException instead of returning a value.
}

// inputs of CreateCustomerCommandHandler -
// CreateCustomerCommand - a command object that contains the necessary information to create a new customer, such as name, email, phone, and address.
// CancellationToken - a cancellation token that can be used to cancel the operation if needed.

// process of CreateCustomerCommandHandler -
// The CreateCustomerCommandHandler class implements the IRequestHandler interface from MediatR, which allows it to handle requests of type CreateCustomerCommand and return a response of type Guid. 
//When a CreateCustomerCommand is sent through the MediatR pipeline, the Handle method of this class is called. 
//This method performs the necessary logic to create a new customer, including checking for existing customers with the same email, creating a new Customer entity, adding it to the database, and returning the ID of the newly created customer.

// outputs of CreateCustomerCommandHandler -
// The output of the CreateCustomerCommandHandler is a Guid, which is the ID of the newly created customer. 
//If a customer with the same email already exists, it throws a BusinessRuleException instead of returning a value.



// process of CreateCustomerCommand -
// a record type that represents a command to create a new customer.
// It contains the necessary information such as name, email, phone, and address. 
// When an instance of this command is created and sent through the MediatR pipeline, it will be handled by the CreateCustomerCommandHandler, which will perform the logic to create a new customer

// outputs of CreateCustomerCommand -
// The output of the CreateCustomerCommand is a Guid, which is the ID of the newly created customer.


// A Handler class is a class that implements the IRequestHandler interface from MediatR. It is responsible for handling a specific type of request (command or query) and performing the necessary logic to process that request. 
//In this case, the CreateCustomerCommandHandler is a Handler class that handles the CreateCustomerCommand and contains the logic to create a new customer in the system. The Handler class typically interacts with repositories, services, or other components to perform its tasks and returns a response based on the request it handles.

// A record is a reference type in C# that provides built-in functionality for immutability, value-based equality, and concise syntax for defining data objects. It take inputs in the form of parameters in its declaration, and automatically generates properties, constructors, and methods for equality comparison, and give outputs based on the values of those properties. 