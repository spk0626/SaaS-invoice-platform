import {
  HttpInterceptorFn,
  HttpRequest,
  HttpHandlerFn,
  HttpErrorResponse
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (        // Interceptor function that will be called for every HTTP request made by the Angular app. It will add the JWT access token to the Authorization header of outgoing requests and handle token refresh logic on 401 responses.
  req: HttpRequest<unknown>,    // The outgoing HTTP request. It is immutable, so we will clone it to add the Authorization header.
  next: HttpHandlerFn      // Function that sends the request to the next interceptor in the chain or to the backend if this is the last interceptor. It takes an HttpRequest and returns an Observable of HttpEvent.
) => {
  const authService = inject(AuthService);

  const authReq = addToken(req, authService.getAccessToken());  // Get the current access token from the AuthService and add it to the request using the addToken helper function. This will clone the request and set the Authorization header if the token is present.

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      // Only attempt refresh for 401s on non-auth endpoints
      if (error.status === 401 && !req.url.includes('/auth/')) {
        return authService.refreshToken().pipe(   // If we get a 401 Unauthorized response, we attempt to refresh the token using the AuthService's refreshToken method. This will call the backend to get a new access token using the refresh token.
          switchMap(response => { // If the refresh is successful, we get a new access token in the response. We then add this new token to the original request and retry it by calling next() again.
            return next(addToken(req, response.accessToken));
          }),
          catchError(refreshError => {
            authService.logout();
            return throwError(() => refreshError);
          })
        );
      }

      return throwError(() => error);
    })
  );
};

function addToken(
  req: HttpRequest<unknown>,
  token: string | null
): HttpRequest<unknown> {
  if (!token) return req;
  return req.clone({
    setHeaders: { Authorization: `Bearer ${token}` }
  });
}
// inputs: 
// an HttpRequest and a token string. 
// Process: 
// If the token is null, it returns the original request unmodified. If the token is present, it clones the request and sets the Authorization header to "Bearer <token>". 
// Output: 
// This cloned request with the Authorization header is then returned and sent to the next handler in the chain.