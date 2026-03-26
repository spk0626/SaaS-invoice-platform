import { Injectable, signal, computed, inject } from '@angular/core';  // Angular 16+ features: signals and dependency injection
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
    AuthResponse,
    LoginRequest,
    RegisterRequest,
    TokenPayload
} from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private readonly http = inject(HttpClient);
    private readonly router = inject(Router);

    private readonly ACCESS_TOKEN_KEY = 'invoice_access_token';
    private readonly REFRESH_TOKEN_KEY = 'invoice_refresh_token';

   // Reactive state using Angular signals
   private readonly _isAuthenticated = signal<boolean>(this.hasValidToken());
   private readonly _currentUser = signal<TokenPayload | null>(this.decodeCurrentToken());

   readonly isAuthenticated =  computed(() => this._isAuthenticated());
    readonly currentUser = computed(() => this._currentUser());
    readonly currentUserEmail = computed(() => this._currentUser()?.email ?? null);

login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
        .post<AuthResponse>(`${environment.apiUrl}/auth/login`, request)
        .pipe(tap(response => this.handleAuthResponse(response)));
}

register(request: RegisterRequest): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${environment.apiUrl}/auth/register`, request);
}

logout(): void {
    this.clearTokens();
    this._isAuthenticated.set(false);
    this._currentUser.set(null);
    this.router.navigate(['/login']);
}

refreshToken(): Observable<AuthResponse> {
    const refreshToken = localStorage.getItem(this.REFRESH_TOKEN_KEY);
    return this.http
        .post<AuthResponse>(`${environment.apiUrl}/auth/refresh`, { refreshToken })
        .pipe(tap(response => this.handleAuthResponse(response)));
}

  getAccessToken(): string | null {
    return localStorage.getItem(this.ACCESS_TOKEN_KEY);
  }

private handleAuthResponse(response: AuthResponse): void {
    localStorage.setItem(this.ACCESS_TOKEN_KEY, response.accessToken);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, response.refreshToken);
    this._isAuthenticated.set(true);
    this._currentUser.set(this.decodeToken(response.accessToken));
}

private clearTokens(): void {
    localStorage.removeItem(this.ACCESS_TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
}

private hasValidToken(): boolean {
    const token = localStorage.getItem(this.ACCESS_TOKEN_KEY);
    if (!token) return false;
    const payload = this.decodeToken(token);
    return payload ? payload.exp * 1000 > Date.now() : false;  // Check if token is expired
}

private decodeCurrentToken(): TokenPayload | null {
    const token = localStorage.getItem(this.ACCESS_TOKEN_KEY);
    return token ? this.decodeToken(token) : null;
}

private decodeToken(token: string): TokenPayload | null {
    try {
        const base64 = token.split('.')[1]
        .replace(/-/g, '+')
        .replace(/_/g, '/');
        return JSON.parse(atob(base64)) as TokenPayload;
    } catch {
        return null;
    }
}

}
