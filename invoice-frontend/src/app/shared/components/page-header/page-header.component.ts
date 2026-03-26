import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-page-header',
  standalone: true,
  imports: [CommonModule, RouterLink, MatButtonModule, MatIconModule],
  template: `
    <div class="page-header">
      <div class="page-header-text">
        <h1 class="page-title">{{ title }}</h1>
        <p class="page-subtitle" *ngIf="subtitle">{{ subtitle }}</p>
      </div>
      <button
        *ngIf="actionLabel && actionRoute"
        mat-flat-button
        color="primary"
        [routerLink]="actionRoute">
        <mat-icon>{{ actionIcon ?? 'add' }}</mat-icon>
        {{ actionLabel }}
      </button>
    </div>
  `
})
export class PageHeaderComponent {
  @Input({ required: true }) title!: string;
  @Input() subtitle?: string;
  @Input() actionLabel?: string;
  @Input() actionRoute?: string | string[];
  @Input() actionIcon?: string;
}