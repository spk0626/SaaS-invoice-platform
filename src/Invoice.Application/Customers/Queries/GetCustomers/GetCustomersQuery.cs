using AutoMapper;
using Invoice.Application.Common.DTOs;
using Invoice.Domain.Interfaces;
using MediatR;

namespace Invoice.Application.Customers.Queries.GetCustomers;

public sealed record GetCustomersQuery(   // defines the types of the input parameters for the query and the expected output type
    int Page = 1,
    int PageSize = 20,
    string? Search = null
) : IRequest<PagedResult<CustomerDto>>;
// input: page, pageSize, search (optional)
// output: PagedResult<CustomerDto> (list of customers, total count, page info)

public sealed class GetCustomersQueryHandler
:IRequestHandler<GetCustomersQuery, PagedResult<CustomerDto>>
{
    private readonly ICustomerRepository _customers;
    private readonly IMapper _mapper;

    public GetCustomersQueryHandler(
        ICustomerRepository customers, IMapper mapper)
    {
        _customers = customers;
        _mapper = mapper;
    }

    public async Task<PagedResult<CustomerDto>> Handle(
        GetCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _customers.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.Search,
            cancellationToken);

            var dtos = _mapper.Map<IEnumerable<CustomerDto>>(items);

            return PagedResult<CustomerDto>.Create(
                dtos, request.Page, request.PageSize, totalCount);
    }
    // inputs of Handle method -
    // GetCustomersQuery request - a query object that contains the pagination and search parameters for retrieving
    // CancellationToken cancellationToken - a cancellation token that can be used to cancel the operation if needed.

    // process:
    // 1. The method calls the GetPagedAsync method of the ICustomerRepository to retrieve a paginated list of customers based on the provided page number, page size, and optional search term. This method returns a tuple containing the list of customer entities and the total count of customers that match the criteria.
    // 2. The retrieved customer entities are then mapped to a list of CustomerDto objects
    // 3. Finally, a PagedResult<CustomerDto> is created using the mapped DTOs, along with the pagination information (current page, page size, and total count), and returned as the result of the query.

    // outputs:
    // The method returns a PagedResult<CustomerDto>, which is a data transfer object that contains a list of CustomerDto objects, the total count of customers that match the criteria, and pagination information such as the current page and page size. This allows the caller to easily display the paginated list of customers along with relevant metadata.
 }
// inputs of GetCustomersQueryHandler class -
// GetCustomersQuery request - a query object that contains the pagination and search parameters for retrieving customers
// CancellationToken cancellationToken - a cancellation token that can be used to cancel the operation if needed.

// process of GetCustomersQueryHandler class -
// The GetCustomersQueryHandler class implements the IRequestHandler interface from MediatR, which allows it to handle requests of type GetCustomersQuery and return a PagedResult<CustomerDto>. 

// Outputs of GetCustomersQueryHandler class -
// returns a PagedResult<CustomerDto>

/*
public sealed class TheClass <TRequest, TResponse>
: IPipelineBehavior<TRequest, TResponse>

public sealed class GetCustomersQueryHandler
:IRequestHandler<GetCustomersQuery, PagedResult<CustomerDto>>

where TRequest : IRequest<TResponse>

TRequest - GetCustomersQuery (the type of the request being handled)
TResponse - PagedResult<CustomerDto>> (the type of the response being returned by the handler)
*/