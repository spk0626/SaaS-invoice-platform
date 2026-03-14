using System.Reflection.Metadata.Ecma335;
using Invoice.Domain.Entities;

namespace Invoice.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default); // retrieves a customer by their id

    Task<Customer?> GetByEmailAsync(string email, CancellationToken ct = default);

    Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(                  // retrieves a paginated list of customers based on the specified parameters. 
        int page, int pageSize, string? search = null, CancellationToken ct = default);

    Task AddAsync(Customer customer, CancellationToken ct = default); // adds a new customer to the repository

    Task UpdateAsync(Customer customer, CancellationToken ct = default); // updates an existing customer's information in the repository

    Task DeleteAsync(Guid id, CancellationToken ct = default); // deletes a customer from the repository by their id
}

// cancellation token allows the operation to be cancelled if needed, such as when a user navigates away from a page or when a request times out.