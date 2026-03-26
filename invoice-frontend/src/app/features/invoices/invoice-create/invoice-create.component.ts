import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import {
  FormBuilder,
  FormArray,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  AbstractControl
} from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { MatTooltipModule } from '@angular/material/tooltip';
import { InvoiceService } from '../../../core/services/invoice.service';
import { CustomerService } from '../../../core/services/customer.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Customer } from '../../../core/models/customer.models';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-invoice-create',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    MatTooltipModule,
    PageHeaderComponent
  ],
  templateUrl: './invoice-create.component.html'
})
export class InvoiceCreateComponent implements OnInit {
  private readonly fb = inject(FormBuilder);     // FormBuilder is an Angular service that provides a convenient way to create reactive forms. It is injected into the component using Angular's dependency injection system, allowing the component to use it to build the form group and form controls for the invoice creation form.
  private readonly invoiceService = inject(InvoiceService);
  private readonly customerService = inject(CustomerService);
  private readonly notifications = inject(NotificationService);
  private readonly router = inject(Router);

  readonly customers = signal<Customer[]>([]);
  readonly submitting = signal(false);
  readonly minDate = new Date();

  readonly currencies = ['USD', 'EUR', 'GBP', 'LKR', 'AUD', 'CAD', 'SGD'];

  form = this.fb.group({
    customerId: ['', Validators.required],
    dueDate:    [null as Date | null, Validators.required],
    currency:   ['USD', Validators.required],
    taxRate:    [0.1, [Validators.required, Validators.min(0), Validators.max(1)]],
    notes:      ['' as string | null],
    items:      this.fb.array([this.buildItemGroup()])
  });

  get itemsArray(): FormArray {
    return this.form.get('items') as FormArray;
  }  // Gets the form array for the line items. Because the form is structured with a nested FormArray for the invoice items, this getter provides a convenient way to access that array and perform operations like adding or removing line items from the invoice form

  get subtotal(): number {
    return this.itemsArray.controls.reduce((sum, ctrl) => {
      const q = ctrl.get('quantity')?.value ?? 0;
      const p = ctrl.get('unitPrice')?.value ?? 0;
      return sum + q * p;
    }, 0);
  }  // Calculates the subtotal by iterating over each line item in the form array, multiplying the quantity and unit price for each item, and summing them up to get the total before tax.
  // ctrl.get is used to access form controls within a FormGroup or FormArray

  get taxAmount(): number {
    return this.subtotal * (this.form.get('taxRate')?.value ?? 0);
  }
  // get is a TypeScript getter. It allows you to define a property (in this case, taxAmount) that is computed dynamically based on other properties (subtotal and taxRate). Whenever you access invoiceCreateComponent.taxAmount, it will execute the code in this getter to calculate the tax amount based on the current subtotal and tax rate from the form.

  get total(): number {
    return this.subtotal + this.taxAmount;
  }

  get selectedCurrency(): string {
    return this.form.get('currency')?.value ?? 'USD';
  }

  ngOnInit(): void {
    this.customerService.getAll({ pageSize: 100 }).subscribe(result => {
      this.customers.set(result.items);
    });
  }
  // ngOnInit is a lifecycle hook in Angular that is called after the component has been initialized. In this method, we are making an API call to fetch the list of customers using the CustomerService. The getAll method is called with a pageSize of 100 to retrieve up to 100 customers. Once the data is received, we update the customers signal with the retrieved list of customers, which can then be used in the template to populate a dropdown or selection list for choosing a customer when creating an invoice.

  private buildItemGroup(): FormGroup {
    return this.fb.group({
      description: ['', [Validators.required, Validators.maxLength(500)]],
      quantity:    [1,  [Validators.required, Validators.min(0.01)]],
      unitPrice:   [0,  [Validators.required, Validators.min(0.01)]]
    });
  } // This method creates and returns a new FormGroup for an invoice line item. Each line item has three form controls: description, quantity, and unitPrice. The description is a required string with a maximum length of 500 characters, while quantity and unitPrice are required numbers with a minimum value of 0.01. This method is used to initialize the form array for invoice items and can also be called when adding new line items to the invoice form.


  lineTotal(index: number): number {
    const ctrl = this.itemsArray.at(index);  // This line retrieves the FormGroup for a specific line item in the items FormArray based on the provided index
    return (ctrl.get('quantity')?.value ?? 0) *
           (ctrl.get('unitPrice')?.value ?? 0);
  }
  // .at() is a method of FormArray that retrieves the FormGroup at the specified index in the FormArray. In this case, it is used to get the FormGroup for a specific line item based on its index, allowing us to access the quantity and unit price for that line item to calculate the line total.

  addItem(): void {
    this.itemsArray.push(this.buildItemGroup());
  }
  // .push() is a method of FormArray that adds a new FormGroup to the array

  removeItem(index: number): void {
    if (this.itemsArray.length > 1) {
      this.itemsArray.removeAt(index);
    }
  }
  // .removeAt() is a method of FormArray that removes the FormGroup at the specified index

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    } // if the form is invalid, we mark all controls as touched to trigger validation messages in the UI and prevent submission until the user corrects the errors.

    this.submitting.set(true);
    const raw = this.form.getRawValue();  // getRawValue() is a method of FormGroup that retrieves the current value of the form, including disabled controls. It returns a plain JavaScript object with the values of all form controls, which can then be used to construct the payload for the API call to create a new invoice. In this case, we are using the raw form values to build the request body for the invoice creation API endpoint.

    this.invoiceService.create({
      customerId: raw.customerId!,
      dueDate:    (raw.dueDate as Date).toISOString(),
      currency:   raw.currency!,
      taxRate:    raw.taxRate!,
      notes:      raw.notes || null,
      items:      raw.items.map((i: any) => ({
        description: i.description,
        quantity:    i.quantity,
        unitPrice:   i.unitPrice
      }))
    }).subscribe({
      next: () => {
        this.notifications.success('Invoice created successfully.');
        this.router.navigate(['/invoices']);
      },
      error: () => this.submitting.set(false)
    });
  }
}