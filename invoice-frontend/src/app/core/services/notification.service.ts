import { Injectable, inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';  // MatSnackBar is used for showing notifications to the user

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly snackBar = inject(MatSnackBar);  // Injecting MatSnackBar using Angular's dependency injection. Because this service is provided in root, it will be a singleton and the same instance of MatSnackBar will be used throughout the app.

  success(message: string): void {
    this.snackBar.open(message, 'Close', {   // Show a success notification with the provided message and a "Close" action
      duration: 3000,                     // The notification will automatically disappear after 3 seconds
      panelClass: ['snack-success'],
      horizontalPosition: 'right',
      verticalPosition: 'top'
    });
  }  // The success method shows a green notification with the provided message. It will automatically disappear after 3 seconds, and it has a "Close" action for manual dismissal. The notification is styled with the 'snack-success' class, which should be defined in the global styles to give it a green background or other success-related styling.

  error(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 5000,
      panelClass: ['snack-error'],
      horizontalPosition: 'right',
      verticalPosition: 'top'
    });
  }  // The error method shows a red notification with the provided message. It will automatically disappear after 5 seconds, and it has a "Close" action for manual dismissal. The notification is styled with the 'snack-error' class, which should be defined in the global styles to give it a red background or other error-related styling.

  info(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top'
    });
  }
}