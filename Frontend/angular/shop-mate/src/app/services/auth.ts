import { HttpClient } from '@angular/common/http';
import { computed, inject, Service, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ShoppingListService } from './shopping-list';
import { AuthUser, LoginResponse } from '../Model/AuthData';
import { Observable, tap } from 'rxjs';
import { StateService } from './state-service';
import { API_BASE_URL } from './api-config';

@Service()
export class AuthService {
  private readonly url = `${API_BASE_URL}/auth`;

  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly lists = inject(ShoppingListService);
  private readonly storage = inject(StateService);

  // Restore before a route guard checks the session on a browser refresh.
  private readonly session = signal<LoginResponse | null>(this.storage.getValue());
  readonly user = computed<AuthUser | null>(() => this.session()?.user ?? null);

  login(username: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.url}/login`, {
        username: username.trim(),
        password,
      })
      .pipe(
        tap((response) => {
          this.lists.reset();
          this.storage.setValue(response);
          this.session.set(response);
        }),
      );
  }

  register(fullname: string, username: string, password: string): Observable<AuthUser> {
    return this.http.post<AuthUser>(`${this.url}/register`, {
      fullName: fullname.trim(),
      username: username.trim(),
      password,
    });
  }

  getToken(): string | null {
    return this.isAuthenticated() ? this.session()?.accessToken ?? null : null;
  }

  isAuthenticated(): boolean {
    const session = this.session();
    if (session === null) return false;
    if (Date.parse(session.expiresAtUtc) > Date.now()) return true;

    this.clearSession();
    return false;
  }

  logout(): void {
    this.clearSession();
    void this.router.navigateByUrl('/login');
  }

  private clearSession(): void {
    this.session.set(null);
    this.lists.reset();
    this.storage.deleteValue();
  }
}
