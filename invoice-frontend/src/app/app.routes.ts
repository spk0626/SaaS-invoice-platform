import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login.component')
        .then(m => m.LoginComponent)
  },
  {
    path: '',
    loadComponent: () =>
      import('./layout/shell/shell.component')
        .then(m => m.ShellComponent),
    canActivate: [authGuard],
    children: [              // Define child routes that will be rendered inside the ShellComponent. These routes are protected by the authGuard, meaning the user must be authenticated to access them.
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/dashboard/dashboard.component')
            .then(m => m.DashboardComponent)
      },
    //   {
    //     path: 'invoices',
    //     children: [
    //       {
    //         path: '',
    //         loadComponent: () =>
    //           import('./features/invoices/invoice-list/invoice-list.component')
    //             .then(m => m.InvoiceListComponent)
    //       },
    //       {
    //         path: 'create',
    //         loadComponent: () =>
    //           import('./features/invoices/invoice-create/invoice-create.component')
    //             .then(m => m.InvoiceCreateComponent)
    //       }
    //     ]
    //   },
    //   {
    //     path: 'customers',
    //     loadComponent: () =>
    //       import('./features/customers/customer-list/customer-list.component')
    //         .then(m => m.CustomerListComponent)
    //   }
    ]
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];