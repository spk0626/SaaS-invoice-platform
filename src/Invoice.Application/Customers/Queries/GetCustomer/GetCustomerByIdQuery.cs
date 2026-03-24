using AutoMapper;
using Invoice.Application.Common.DTOs;
using Invoice.Domain.Exceptions;
using Invoice.Domain.Interfaces;
using MediatR;

namespace Invoice.Application.Customers.Queries.GetCustomer;

public sealed record GetCustomerByIdQuery(Guid Id): IRequest<CustomerDto>;  // the one that get relevant data and prepares the output data shape for the Handler class input. It takes a Guid Id as input and returns a CustomerDto as output.

public sealed class GetCustomerByIdQueryHandler
: IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly ICustomerRepository _customers;  // A repository interface for accessing customer data. To retrieve the customer information from the database.
    private readonly IMapper _mapper;  // An instance of AutoMapper's IMapper interface, which is used to map the retrieved customer entity to a CustomerDto.

    public GetCustomerByIdQueryHandler(
        ICustomerRepository customers, IMapper mapper)
    {
        _customers = customers;  // Constructor that takes an ICustomerRepository and an IMapper instance and assigns them to the private readonly fields
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(
        GetCustomerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await _customers.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Customer", request.Id);  // Retrieve the customer from the database using the provided ID. If the customer is not found, throw a NotFoundException.
        
        return _mapper.Map<CustomerDto>(customer);
    }
    // inputs of Handle method -
    // GetCustomerByIdQuery request - a query object that contains the ID of the customer to be retrieved.
    // CancellationToken cancellationToken - a cancellation token that can be used to cancel the operation if needed.

    // process:
    // 1. The method first attempts to retrieve the customer from the database using the GetByIdAsync method of the ICustomerRepository, passing the customer's ID from the request and the cancellation token.
    // 2. If the customer is not found (i.e., the result is null), it throws a NotFoundException with the appropriate message.
    // 3. If the customer is found, it uses AutoMapper to map the retrieved customer entity to a CustomerDto and returns it.

    // outputs:
    // The method returns a CustomerDto, which is a data transfer object that contains the relevant information about the customer. If the customer with the specified ID is not found, it throws a NotFoundException instead of returning a value.


}
// inputs of GetCustomerByIdQueryHandler class -
// GetCustomerByIdQuery request - a query object that contains the ID of the customer to be retrieved.
// CancellationToken cancellationToken - a cancellation token that can be used to cancel the operation if needed.

// process of GetCustomerByIdQueryHandler class -
// The GetCustomerByIdQueryHandler class implements the IRequestHandler interface from MediatR, which allows it to handle requests of type GetCustomerByIdQuery and return a CustomerDto. The Handle method retrieves the customer from the database using the provided ID, checks if the customer exists, and if so, maps it to a CustomerDto using AutoMapper before returning it. If the customer is not found, it throws a NotFoundException.

// outputs of GetCustomerByIdQueryHandler class -
// The method returns a CustomerDto, which is a data transfer object that contains the relevant information 


/* Format:
public sealed class TheClass <TRequest, TResponse>
: IPipelineBehavior<TRequest, TResponse>
eg; - 
public sealed class GetCustomerByIdQueryHandler
: IRequestHandler<GetCustomerByIdQuery, CustomerDto>

where TRequest : IRequest<TResponse>
eg; - record GetCustomerByIdQuery: IRequest<CustomerDto>
*/

// Flow:
// 1. A GetCustomerByIdQuery is created with a specific customer ID and sent through the MediatR pipeline.
// 2. The GetCustomerByIdQueryHandler class is invoked to handle the query.
// 3. Constructor of GetCustomerByIdQueryHandler is called to initialize the handler with the necessary dependencies (ICustomerRepository and IMapper).
// 4. The handler retrieves the customer from the database using the provided ID.
// 5. If the customer is not found, a NotFoundException is thrown.
// 6. If the customer is found, it is mapped to a CustomerDto using AutoMapper.
// 7. The CustomerDto is returned as the response to the query.
