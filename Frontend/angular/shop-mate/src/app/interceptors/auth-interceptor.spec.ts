import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { Component } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { AuthService } from '../services/auth';
import { ShoppingListService } from '../services/shopping-list';
import { StateService } from '../services/state-service';
import { authInterceptor } from './auth-interceptor';

@Component({ template: '' })
class EmptyLoginPage {}

describe('authInterceptor', () => {
  let client: HttpClient;
  let http: HttpTestingController;
  let auth: AuthService;

  beforeEach(() => {
    localStorage.clear();
    TestBed.configureTestingModule({
      providers: [
        AuthService,
        ShoppingListService,
        StateService,
        provideRouter([{ path: 'login', component: EmptyLoginPage }]),
        provideHttpClient(withInterceptors([authInterceptor])),
        provideHttpClientTesting(),
      ],
    });
    client = TestBed.inject(HttpClient);
    http = TestBed.inject(HttpTestingController);
    auth = TestBed.inject(AuthService);
  });

  afterEach(() => {
    http.verify();
    localStorage.clear();
  });

  function signIn(): void {
    auth.login('learner', 'Classroom1!').subscribe();
    const request = http.expectOne('https://localhost:7027/api/auth/login');
    expect(request.request.headers.has('Authorization')).toBe(false);
    request.flush({
      accessToken: 'classroom-token',
      expiresAtUtc: new Date(Date.now() + 60_000).toISOString(),
      user: { id: 1, fullName: 'Class Learner', username: 'learner', role: 'User' },
    });
  }

  it('attaches a token only to protected ShopMate API requests', () => {
    signIn();

    client.get('https://localhost:7027/api/shopping-lists').subscribe();
    const protectedRequest = http.expectOne('https://localhost:7027/api/shopping-lists');
    expect(protectedRequest.request.headers.get('Authorization')).toBe('Bearer classroom-token');
    protectedRequest.flush([]);

    client.get('https://other.example/api/shopping-lists').subscribe();
    const otherOrigin = http.expectOne('https://other.example/api/shopping-lists');
    expect(otherOrigin.request.headers.has('Authorization')).toBe(false);
    otherOrigin.flush([]);

    client.get('https://localhost:7027/health').subscribe();
    const outsideApi = http.expectOne('https://localhost:7027/health');
    expect(outsideApi.request.headers.has('Authorization')).toBe(false);
    outsideApi.flush({ status: 'ok' });
  });

  it('does not attach an old token to registration', () => {
    signIn();

    client.post('https://localhost:7027/api/auth/register', {}).subscribe();
    const request = http.expectOne('https://localhost:7027/api/auth/register');
    expect(request.request.headers.has('Authorization')).toBe(false);
    request.flush({});
  });

  it('clears the session on a protected 401 but not on a 403', () => {
    signIn();

    client.get('https://localhost:7027/api/shopping-lists').subscribe({ error: () => undefined });
    http.expectOne('https://localhost:7027/api/shopping-lists').flush(null, {
      status: 403,
      statusText: 'Forbidden',
    });
    expect(auth.getToken()).toBe('classroom-token');

    client.get('https://localhost:7027/api/shopping-lists').subscribe({ error: () => undefined });
    http.expectOne('https://localhost:7027/api/shopping-lists').flush(null, {
      status: 401,
      statusText: 'Unauthorized',
    });
    expect(auth.getToken()).toBeNull();
  });
});
