import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import {
  provideHttpClient,
  withInterceptors,
  withFetch
} from '@angular/common/http';
import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { errorInterceptor } from './core/interceptors/error.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),   // This configures Angular's change detection strategy to coalesce multiple events into a single change detection cycle, improving performance by reducing the number of times the UI is updated in response to rapid events (e.g., user input, HTTP responses).
    provideRouter(routes, withComponentInputBinding()),   // This sets up the Angular router with the defined routes and enables automatic input binding for route components, allowing route parameters to be passed directly as component inputs.
    provideHttpClient(  // This configures the Angular HttpClient with global settings. In this case, we are adding two interceptors (authInterceptor and errorInterceptor) that will be applied to all HTTP requests made by the HttpClient. The withFetch() option allows the HttpClient to use the Fetch API under the hood for making HTTP requests, which can provide better performance and more modern features compared to XMLHttpRequest.
      withFetch(),
      withInterceptors([authInterceptor, errorInterceptor])
    ),
    provideAnimationsAsync(),                // This enables support for Angular's animation system, allowing the application to use animations in its components and templates. The "Async" version of this provider allows for lazy loading of the animation module, which can improve initial load performance by only loading the animation code when it's actually needed.
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' }
  ]
};
