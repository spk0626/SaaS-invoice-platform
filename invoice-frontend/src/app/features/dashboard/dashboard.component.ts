import { Component, OnInit, inject, signal } from '@angular/core';  // signal is used for reactive state management in Angular 16+. OnInit is a lifecycle hook that is called after the component is initialized, and it is used here to fetch data for the dashboard when the component loads.
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { forkJoin } from 'rxjs';
import { InvoiceService } from '../../core/services/invoice.service';
import { CustomerService } from '../../core/services/customer.service';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header.component';

interface DashboardStats {     // Define the structure for dashboard statistics
  totalInvoices: number;
  totalCustomers: number;
  sentCount: number;
  overdueCount: number;
}

interface StatCard {          // Define the structure for the data needed to display each statistic card on the dashboard. Each card will have a label (e.g., "Total Invoices"), a value (the actual statistic number), an icon (the name of the Material icon to display), and a color (the background color for the card). The DashboardComponent will use an array of StatCard objects to render the statistic cards dynamically based on the data retrieved from the services.
  label: string;
  value: number;
  icon: string;
  color: string;
}

@Component({                            // This is the main component for the dashboard page. It displays key statistics about invoices and customers, such as total invoices, total customers, invoices awaiting payment, and overdue invoices. The component uses Angular's OnInit lifecycle hook to fetch data from the InvoiceService and CustomerService when the component is initialized. It uses forkJoin to make multiple API calls in parallel and then processes the results to populate the dashboard statistics and cards. The template for this component will display a loading spinner while data is being fetched and will show the statistics in a card format once the data is available.
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    PageHeaderComponent
  ],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {                
  private readonly invoiceService = inject(InvoiceService);
  private readonly customerService = inject(CustomerService);

  readonly loading = signal(true);          // This signal is used to track the loading state of the dashboard. It is initialized to true, indicating that the data is being loaded. Once the data fetching is complete (either successfully or with an error), this signal will be set to false, which can be used in the template to conditionally display a loading spinner or the actual dashboard content based on whether the data is still being fetched or has been loaded.
  readonly stats = signal<DashboardStats | null>(null);    // This signal holds the dashboard statistics data. It is initialized to null and will be updated with the actual statistics once the data is fetched from the services. The template can use this signal to conditionally display the statistics or show a loading state while the data is being retrieved.

  readonly statCards = signal<StatCard[]>([]);

  ngOnInit(): void {                // The ngOnInit lifecycle hook is used to perform component initialization logic. In this case, it is used to fetch the necessary data for the dashboard when the component is first loaded. The method makes parallel API calls to retrieve the total number of invoices, the number of sent invoices, the number of overdue invoices, and the total number of customers. Once all the data is retrieved, it processes the results to populate the stats and statCards signals, which are then used in the template to display the dashboard statistics. If there is an error during data fetching, it ensures that the loading state is set to false so that the UI can reflect that data loading has completed (even if it failed).
    forkJoin({               // forkJoin is an RxJS operator that allows you to execute multiple observables in parallel and wait for all of them to complete before processing the results. In this case, it is used to make multiple API calls to fetch different pieces of data needed for the dashboard (total invoices, sent invoices, overdue invoices, and total customers). The results from all these calls are then combined into a single object that can be easily processed to update the dashboard statistics and cards.
      all: this.invoiceService.getAll({ pageSize: 1 }),
      sent: this.invoiceService.getAll({ status: 'Sent', pageSize: 1 }),
      overdue: this.invoiceService.getAll({ status: 'Overdue', pageSize: 1 }),
      customers: this.customerService.getAll({ pageSize: 1 })
    }).subscribe({                                    // The subscribe method is used to handle the asynchronous response from the forkJoin observable. It takes an object with next and error callbacks. The next callback is called when the data is successfully retrieved, and it processes the results to update the stats and statCards signals with the new data. The error callback is called if there is an error during the data fetching process, and it ensures that the loading state is set to false so that the UI can reflect that data loading has completed (even if it failed).   Is this obeserver design pattern? Yes, it is. The subscribe method is part of the Observable pattern in RxJS, which allows you to react to asynchronous data streams. In this case, the component is subscribing to the observable returned by forkJoin, and it reacts to the emitted data (or error) accordingly.
      next: results => {
        const data: DashboardStats = {
          totalInvoices: results.all.totalCount,
          totalCustomers: results.customers.totalCount,
          sentCount: results.sent.totalCount,
          overdueCount: results.overdue.totalCount
        };

        this.stats.set(data);
        this.statCards.set([
          {
            label: 'Total invoices',
            value: data.totalInvoices,
            icon: 'receipt_long',
            color: '#1565c0'
          },
          {
            label: 'Customers',
            value: data.totalCustomers,
            icon: 'people',
            color: '#2e7d32'
          },
          {
            label: 'Awaiting payment',
            value: data.sentCount,
            icon: 'pending_actions',
            color: '#e65100'
          },
          {
            label: 'Overdue',
            value: data.overdueCount,
            icon: 'warning_amber',
            color: '#c62828'
          }
        ]);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }  // this uses 'The Observer Design Pattern' because the component is subscribing to an observable (the result of forkJoin) and reacting to the emitted data or error. The subscribe method allows the component to handle asynchronous data streams and update its state accordingly when new data is received or when an error occurs.
}