// The shell wraps all authenticated pages. It provides the sidebar and toolbar.

import { Component, inject, computed, signal } from '@angular/core';
import {
  RouterOutlet,
  RouterLink,
  RouterLinkActive
} from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthService } from '../../core/services/auth.service';

interface NavItem {  // This interface defines the structure of a navigation item that will be displayed in the sidebar of the ShellComponent. Each NavItem has a label (the text to display), an icon (the name of the Material icon to show), and a route (the path to navigate to when the item is clicked). The ShellComponent will use an array of NavItem objects to render the navigation menu in the sidebar, allowing users to navigate between different sections of the application (e.g., Dashboard, Invoices, Customers).
  label: string;
  icon: string;
  route: string;
}

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    MatListModule,
    MatTooltipModule
  ],
  templateUrl: './shell.component.html'
})          // This is the main layout component for the authenticated part of the application. It includes a toolbar at the top and a sidenav (sidebar) for navigation. The RouterOutlet is used to render the child components based on the current route. The component also defines a list of navigation items that will be displayed in the sidebar, and it uses the AuthService to manage user authentication and provide a logout function. 

// The ShellComponent serves as a wrapper for all authenticated pages, providing a consistent layout and navigation structure across the app.
export class ShellComponent {
  private readonly auth = inject(AuthService);

  readonly userEmail = computed(() => this.auth.currentUserEmail());   // This computed property retrieves the current user's email from the AuthService. It uses Angular's computed function to create a reactive property that will automatically update whenever the underlying authentication state changes (e.g., when the user logs in or out). The userEmail can be displayed in the toolbar or elsewhere in the ShellComponent's template to show the logged-in user's email address, providing a personalized touch to the UI and confirming to the user that they are logged in.
 
  readonly navItems: NavItem[] = [                         // This array defines the navigation items that will be displayed in the sidebar of the ShellComponent. Each item has a label (the text to display), an icon (the name of the Material icon to show), and a route (the path to navigate to when the item is clicked). The navItems array is used in the template to render the navigation menu, allowing users to easily access different sections of the application such as the Dashboard, Invoices, and Customers pages.
    { label: 'Dashboard', icon: 'dashboard', route: '/dashboard' },
    { label: 'Invoices',  icon: 'receipt_long', route: '/invoices' },
    { label: 'Customers', icon: 'people', route: '/customers' }
  ];

  logout(): void {
    this.auth.logout();
  }
}