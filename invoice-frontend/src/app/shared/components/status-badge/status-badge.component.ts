import { Component, Input } from '@angular/core';
import { NgStyle } from '@angular/common';
import { InvoiceStatus } from '../../../core/models/invoice.models';

interface BadgeConfig {
  label: string;
  color: string;
  background: string;
}

const STATUS_MAP: Record<InvoiceStatus, BadgeConfig> = {
  Draft:     { label: 'Draft',     color: '#525252', background: '#f4f4f4' },
  Sent:      { label: 'Sent',      color: '#0043ce', background: '#edf5ff' },
  Paid:      { label: 'Paid',      color: '#198038', background: '#defbe6' },
  Overdue:   { label: 'Overdue',   color: '#a2191f', background: '#fff1f1' },
  Cancelled: { label: 'Cancelled', color: '#6f6f6f', background: '#e8e8e8' }
};

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [NgStyle],
  template: `
    <span
      [ngStyle]="{
        color: config.color,
        background: config.background,
        padding: '2px 10px',
        borderRadius: '12px',
        fontSize: '12px',
        fontWeight: '500',
        display: 'inline-block',
        whiteSpace: 'nowrap'
      }">
      {{ config.label }}
    </span>
  `
})
export class StatusBadgeComponent {
  config: BadgeConfig = STATUS_MAP['Draft'];   // Default to 'Draft' config if no status is set

  @Input({ required: true })       // The status input is required, meaning that the parent component must provide a value for it. This ensures that the badge will always have a valid status to display.
  set status(value: InvoiceStatus) {
    this.config = STATUS_MAP[value] ?? STATUS_MAP['Draft'];
  }
}