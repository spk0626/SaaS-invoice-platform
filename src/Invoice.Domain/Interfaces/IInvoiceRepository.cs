using Invoice.Domain.Enums;
using InvoiceEntity = Invoice.Domain.Entities.Invoice;

namespace Invoice.Domain.Interfaces;

public interface IInvoiceRepository
{
    Task<InvoiceEntity?> GetByIdAsync(Guid id, CancellationToken ct = default); // retrieves an invoice by its id, null if not found.
    
    Task<(IEnumerable<InvoiceEntity> Items, int TotalCount)> GetPagedAsync( // retrieves a paginated list of invoices based on the specified parameters. It returns a tuple containing the list of invoices and the total count of invoices that match the criteria, which is useful for implementing pagination in the user interface.
        int page, 
        int pageSize, 
        Guid? customerId = null, 
        InvoiceStatus? status = null, 
        CancellationToken ct = default); 
        
    Task AddAsync(InvoiceEntity invoice, CancellationToken ct = default); // adds a new invoice to the repository
    Task UpdateAsync(InvoiceEntity invoice, CancellationToken ct = default); // updates an existing invoice in the repository
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default); // checks if an invoice with the specified id exists in the repository
}