using Invoice.Domain.Entities;

namespace Invoice.Domain.Interfaces;

public interface IPaymentRepository
{
    Task<IEnumerable<Payment>> GetByInvoiceIdAsync(
        Guid invoiceId, CancellationToken ct = default); // retrieves all payments associated with a specific invoice id

    Task AddAsync(Payment payment, CancellationToken ct = default); // adds a new payment to the repository
}

// Task is a representation of an asynchronous operation that may produce a result in the future
// // Task is the type of the method AddAsync. It is a class in the System.Threading.Tasks namespace that represents an asynchronous operation. It can be used to represent operations that may complete in the future and can return a result or simply indicate completion.
// the methods in the interface are asynchronous and return Task or Task<T> to indicate that they are asynchronous operations that may produce a result in the future