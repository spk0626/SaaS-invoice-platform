namespace Invoice.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendInvoiceAsync(
        string toEmail,
        string customerName,
        string invoiceNumber,
        decimal total,
        CancellationToken ct = default); // sends an email notification to the customer when an invoice is created or updated. It includes the customer's name, invoice number, and total amount due in the email content.

    Task SendInvoiceReminderAsync(
        string toEmail,
        string customerName,
        string invoiceNumber,
        decimal amountDue,
        DateTime dueDate,
        CancellationToken ct = default); // sends a reminder email to the customer when an invoice is approaching its due date or is overdue. It includes the customer's name, invoice number, amount due, and due date in the email content.
}