import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (auth.isAuthenticated()) return true;

  return router.createUrlTree(['/login']);
};
// inputs: None (this is a guard function that will be used in route definitions).
// Process:
// This guard checks if the user is authenticated by calling the isAuthenticated method of the AuthService. If the user is authenticated, it returns true and allows access to the route. If not, it redirects the user to the login page by returning a UrlTree created with router.createUrlTree(['/login']).
// Output:
// A boolean value (true if authenticated) or a UrlTree that redirects to the login page if not authenticated.


// A guard is a function that determines whether a route can be activated (i.e., accessed) based on certain conditions. In this case, the authGuard checks if the user is authenticated before allowing access to protected routes. If the user is not authenticated, it redirects them to the login page. This is a common pattern in Angular applications to protect routes that require authentication.

// 1. The user tries to access a protected route (e.g., /dashboard).
// 2. The authGuard is invoked because it is specified in the route definition for /dashboard.
// 3. The authGuard checks if the user is authenticated by calling auth.isAuthenticated().
// 4. If the user is authenticated, authGuard returns true, and the user can access the /dashboard route.
// 5. If the user is not authenticated, authGuard returns a UrlTree that redirects to /login.
// The authGuard relies on the AuthService to determine if the user is authenticated. The AuthService manages the authentication state and provides methods for logging in, logging out, and refreshing tokens. The guard uses this service to check the authentication status and make routing decisions accordingly.


// - Guards (like authGuard) are used to protect routes and control access based on conditions (e.g., authentication).
// - Interceptors (like authInterceptor and errorInterceptor) are used to modify HTTP requests and responses globally (e.g., adding auth tokens, handling errors).
// - Services (like AuthService and NotificationService) provide reusable logic and state management for specific concerns (e.g., authentication, notifications). Guards and interceptors can inject these services to perform their tasks (e.g., authGuard injects AuthService to check authentication, authInterceptor injects AuthService to get the access token, errorInterceptor injects NotificationService to show error messages).