using Invoice.Domain.Common;
using Invoice.Domain.Exceptions;
namespace Invoice.Domain.Entities;

public sealed class InvoiceItem: AuditableEntity
{
    public Guid InvoiceId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Quantity { get; private set; } // quantity is decimal because some items may be measured in fractions (e.g. weight)
    public decimal UnitPrice { get; private set; }
    public decimal LineTotal => Math.Round(Quantity * UnitPrice, 2); // total of the line item

    private InvoiceItem() { } 

    public static InvoiceItem Create(
        Guid invoiceId, 
        string description, 
        decimal quantity, 
        decimal unitPrice)
    {
        if (quantity <= 0) 
        throw new BusinessRuleException("Quantity must be greater than zero.");
        
        if (unitPrice <= 0) 
        throw new BusinessRuleException("Unit price must be greater than zero.");
        
        if (string.IsNullOrWhiteSpace(description)) 
        throw new BusinessRuleException("Description is required.");

        return new InvoiceItem
        {
            InvoiceId = invoiceId,
            Description = description.Trim(),
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }
    // inputs: invoiceId, description, quantity, unitPrice
    // process: validate inputs, create new InvoiceItem instance with generated Id and calculated LineTotal
    // outputs: a new InvoiceItem object initialized with the provided values and calculated LineTotal
}