import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-empty-state',
  standalone: true,
  imports: [MatIconModule],
  template: `
    <div class="empty-state">
      <mat-icon class="empty-icon">{{ icon }}</mat-icon>
      <p class="empty-title">{{ title }}</p>
      <p class="empty-subtitle" *ngIf="subtitle">{{ subtitle }}</p>
    </div>
  `
})
export class EmptyStateComponent {
  @Input({ required: true }) title!: string;
  @Input() subtitle?: string;
  @Input() icon = 'inbox';
}
// The EmptyStateComponent is a reusable UI component designed to display a message when there is no data to show. It accepts three inputs: title (required), subtitle (optional), and icon (optional, with a default value of 'inbox'). The component uses Angular Material's MatIconModule to display an icon, and it conditionally renders the subtitle if it is provided. This component can be used across the application to provide a consistent and user-friendly way to indicate empty states in various contexts, such as when there are no invoices, customers, or other data to display.