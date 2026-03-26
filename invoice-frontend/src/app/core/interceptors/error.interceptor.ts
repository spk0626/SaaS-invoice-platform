import {
  HttpInterceptorFn,
  HttpErrorResponse
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { NotificationService } from '../services/notification.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notifications = inject(NotificationService);

  return next(req).pipe(                 
    catchError((error: HttpErrorResponse) => {
      // Auth interceptor handles 401 — skip here
      if (error.status === 401) {
        return throwError(() => error);
      }

      const message = extractMessage(error);
      notifications.error(message);

      return throwError(() => error);
    })
  );
};

function extractMessage(error: HttpErrorResponse): string {
  // Validation errors from FluentValidation
  if (error.error?.errors?.length) {  // Check if the error response has an "errors" array with at least one error message. This is a common structure for validation errors returned by FluentValidation in the backend.
    return error.error.errors[0];
  }

  // Business rule or not found errors from our middleware
  if (error.error?.error) {
    return error.error.error;
  }

  switch (error.status) {
    case 0:    return 'Cannot reach the server. Check your connection.';
    case 403:  return 'You do not have permission to do that.';
    case 404:  return 'The requested resource was not found.';
    case 422:  return error.error?.error ?? 'This action is not allowed.';
    case 429:  return 'Too many requests. Please wait a moment.';
    case 500:  return 'Server error. Please try again later.';
    default:   return 'Something went wrong. Please try again.';
  }
}