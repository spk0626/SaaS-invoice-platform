namespace Invoice.Domain.Enums;

// defines the possible statuses for an invoice in the system. Each status represents a different stage in the invoice lifecycle, such as Draft, Sent, Paid, Overdue, and Cancelled. This enum can be used to track and manage the state of invoices throughout their lifecycle.
public enum InvoiceStatus 
{
    Draft = 0,
    Sent = 1,
    Paid = 2,
    Overdue = 3,
    Cancelled = 4
}