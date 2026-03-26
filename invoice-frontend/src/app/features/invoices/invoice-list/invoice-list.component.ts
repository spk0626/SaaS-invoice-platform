import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatMenuModule } from '@angular/material/menu';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import {
  InvoiceService,
  InvoiceFilters
} from '../../../core/services/invoice.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Invoice, InvoiceStatus } from '../../../core/models/invoice.models';
import { PagedResult } from '../../../core/models/shared.models';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';
import { StatusBadgeComponent } from '../../../shared/components/status-badge/status-badge.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';

@Component({
  selector: 'app-invoice-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatMenuModule,
    MatSelectModule,
    MatFormFieldModule,
    PageHeaderComponent,
    StatusBadgeComponent,
    EmptyStateComponent
  ],
  templateUrl: './invoice-list.component.html'
})
export class InvoiceListComponent implements OnInit {
  private readonly invoiceService = inject(InvoiceService);
  private readonly notifications = inject(NotificationService);

  readonly loading = signal(true);
  readonly result = signal<PagedResult<Invoice> | null>(null);

  displayedColumns = [
    'invoiceNumber',
    'customerName',
    'issueDate',
    'dueDate',
    'total',
    'status',
    'actions'
  ];

  filters: InvoiceFilters = { page: 1, pageSize: 20 };

  readonly statusOptions: { label: string; value: InvoiceStatus | '' }[] = [
    { label: 'All statuses', value: '' },
    { label: 'Draft',        value: 'Draft' },
    { label: 'Sent',         value: 'Sent' },
    { label: 'Paid',         value: 'Paid' },
    { label: 'Overdue',      value: 'Overdue' },
    { label: 'Cancelled',    value: 'Cancelled' }
  ];

  selectedStatus: InvoiceStatus | '' = '';

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.invoiceService.getAll(this.filters).subscribe({
      next: data => {
        this.result.set(data);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  applyStatusFilter(): void {
    this.filters = {
      ...this.filters,
      page: 1,
      status: this.selectedStatus || undefined
    };
    this.load();
  }

  onPage(event: PageEvent): void {
    this.filters = {
      ...this.filters,
      page: event.pageIndex + 1,
      pageSize: event.pageSize
    };
    this.load();
  }

  sendInvoice(invoice: Invoice): void {
    this.invoiceService.send(invoice.id).subscribe({
      next: () => {
        this.notifications.success(
          `Invoice ${invoice.invoiceNumber} sent successfully.`
        );
        this.load();
      }
    });
  }
}