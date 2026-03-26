import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CustomerService } from '../../../core/services/customer.service';
import { Customer } from '../../../core/models/customer.models';
import { PagedResult } from '../../../core/models/shared.models';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';

@Component({
  selector: 'app-customer-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
    PageHeaderComponent,
    EmptyStateComponent
  ],
  templateUrl: './customer-list.component.html'
})
export class CustomerListComponent implements OnInit {
  private readonly customerService = inject(CustomerService);

  readonly loading = signal(true);
  readonly result = signal<PagedResult<Customer> | null>(null);

  displayedColumns = ['name', 'email', 'phone', 'status', 'createdAt'];
  page = 1;
  pageSize = 20;
  searchTerm = '';

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.customerService.getAll({
      page: this.page,
      pageSize: this.pageSize,
      search: this.searchTerm || undefined
    }).subscribe({
      next: data => { this.result.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  search(): void {
    this.page = 1;
    this.load();
  }  // The search method is called when the user initiates a search action, such as clicking a search button or pressing enter in the search input field. It resets the current page to 1 to ensure that the search results start from the first page and then calls the load method to fetch the filtered list of customers based on the current search term. This allows users to quickly find specific customers by name, email, or other criteria defined in the search functionality of the CustomerService.

  clearSearch(): void {
    this.searchTerm = '';
    this.page = 1;
    this.load();
  }  

  onPage(event: PageEvent): void {
    this.page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.load();
  }  // The onPage method is an event handler for pagination events triggered by the MatPaginator component. When the user changes the page or page size, this method is called with a PageEvent object that contains the new page index and page size. The method updates the component's page and pageSize properties accordingly and then calls the load method to fetch the appropriate subset of customers for the new page. This allows users to navigate through multiple pages of customer data efficiently.
}