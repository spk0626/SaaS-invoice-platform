import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  Invoice,
  CreateInvoiceRequest,
  RecordPaymentRequest,
  MonthlyRevenue,
  InvoiceStatus
} from '../models/invoice.models';
import { PagedResult } from '../models/shared.models';

export interface InvoiceFilters {
  page?: number;
  pageSize?: number;
  customerId?: string;
  status?: InvoiceStatus;
}

@Injectable({ providedIn: 'root' })
export class InvoiceService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/invoices`;

  getAll(filters: InvoiceFilters = {}): Observable<PagedResult<Invoice>> {
    let params = new HttpParams()
      .set('page', filters.page ?? 1)        // Default to page 1 if not provided
      .set('pageSize', filters.pageSize ?? 20);     // Default to 20 items per page if not provided

    if (filters.customerId) {
      params = params.set('customerId', filters.customerId); // Filter by customer ID if provided
    }
    if (filters.status) {
      params = params.set('status', filters.status); // Filter by status if provided
    }

    return this.http.get<PagedResult<Invoice>>(this.baseUrl, { params });
  }

  getById(id: string): Observable<Invoice> {
    return this.http.get<Invoice>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateInvoiceRequest): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(this.baseUrl, request);
  }

  send(id: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${id}/send`, {});
  }

  recordPayment(id: string, request: RecordPaymentRequest): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${id}/payments`, request);
  }

  getMonthlyRevenue(year?: number): Observable<MonthlyRevenue[]> {
    let params = new HttpParams();
    if (year) params = params.set('year', year);  // if year is not provided, backend will default to current year
    return this.http.get<MonthlyRevenue[]>(        // endpoint to get monthly revenue data for charts
      `${environment.apiUrl}/reports/revenue`,
      { params }
    );
  }
}