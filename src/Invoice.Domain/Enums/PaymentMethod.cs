namespace Invoice.Domain.Enums;

public enum PaymentStatus { Pending, Completed, Failed, Refunded } // defines the possible payment statuses for an invoice in the system. Each status represents a different state of the payment process 
public enum PaymentMethod { BankTransfer, CreditCard, Cash, Cheque, Other }