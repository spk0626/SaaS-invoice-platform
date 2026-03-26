import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  Customer,
  CreateCustomerRequest,
  UpdateCustomerRequest
} from '../models/customer.models';
import { PagedResult } from '../models/shared.models';

export interface CustomerFilters {
  page?: number;
  pageSize?: number;
  search?: string;
}

@Injectable({ providedIn: 'root' })
export class CustomerService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/customers`;

  getAll(filters: CustomerFilters = {}): Observable<PagedResult<Customer>> {
    let params = new HttpParams()
      .set('page', filters.page ?? 1)
      .set('pageSize', filters.pageSize ?? 20);

    if (filters.search?.trim()) {
      params = params.set('search', filters.search.trim());
    }

    return this.http.get<PagedResult<Customer>>(this.baseUrl, { params });
  }

  getById(id: string): Observable<Customer> {
    return this.http.get<Customer>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateCustomerRequest): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(this.baseUrl, request);
  }

  update(id: string, request: UpdateCustomerRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, request);
  }
}