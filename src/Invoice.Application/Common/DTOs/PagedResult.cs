namespace Invoice.Application.Common.DTOs;

public sealed class PagedResult<T>
{
    public IEnumerable<T> Items { get; init; } = Enumerable.Empty<T>(); // the list of items for the current page, initialized to an empty enumerable to avoid null reference issues.
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; } // the total number of items across all pages, which is essential for implementing pagination in the user interface (e.g., to calculate total pages).
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / (double)PageSize); // calculates the total number of pages based on the total count and page size, using ceiling to ensure that any remaining items are accounted for in an additional page.
    public bool HasNextPage => Page < TotalPages; // to enable/disable pagination controls in the UI.
    public bool HasPreviousPage => Page > 1; // to enable/disable pagination controls in the UI.

    public static PagedResult<T> Create(
        IEnumerable<T> items, 
        int page, 
        int pageSize, 
        int totalCount) =>
        new()                      // factory method to create a new instance of PagedResult<T> 
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
}

// PagedResult<T> class
// T is a type parameter.
// T can be a class, struct, or any other type. 
// eg; PagedResult<CustomerDto>, PagedResult<InvoiceDto>, etc. 