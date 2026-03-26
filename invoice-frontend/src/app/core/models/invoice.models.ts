export type InvoiceStatus = 'Draft' | 'Sent' | 'Paid' | 'Overdue' | 'Cancelled';

export type PaymentMethod = 
| 'Bank Transfer'
| 'Credit Card'
| 'Cash'
| 'Cheque'
| 'Other';

export interface InvoiceItem {
    id: string;
    description: string;
    quantity: number;
    unitPrice: number;
    lineTotal: number; // quantity * unitPrice
}

export interface Payment {
  id: string;
  amount: number;
  paidAt: string;
  status: string;
  method: string;
  reference: string | null;
}

export interface Invoice {
    id: string;
    invoiceNumber: string;
    customerId: string;
    customerName: string;
    issueDate: string; // ISO date string
    dueDate: string; // ISO date string
    statusName: InvoiceStatus;
    currency: string;
    subtotal: number;
    taxAmount: number;
    total: number;
    amountPaid: number;
    amountDue: number;
    notes: string | null;
    items: InvoiceItem[];
    payments: Payment[];
}


export interface CreateInvoiceItemRequest {
    description: string;
    quantity: number;
    unitPrice: number;
}

export interface CreateInvoiceRequest {
    customerId: string;
    dueDate: string; // ISO date string
    currency: string;
    taxRate: number; // Percentage (e.g., 10 for 10%)
    notes?: string | null;
    items: CreateInvoiceItemRequest[];
}

export interface RecordPaymentRequest {
    amount: number;
    method: PaymentMethod;
    reference?: string | null;
}

export interface MonthlyRevenue {
    month: string; // Format: YYYY-MM
    monthName: string; // e.g., "January"
    revenue: number;
    invoiceCount: number;
}