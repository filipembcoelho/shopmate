import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { AuthService } from './auth';
import { ShoppingListService } from './shopping-list';
import { StateService } from './state-service';

describe('StateService', () => {
  beforeEach(() => localStorage.clear());
  afterEach(() => localStorage.clear());

  it('returns null and removes malformed stored data', () => {
    localStorage.setItem('ShopMateSession', '{not json');

    const state = new StateService();
    expect(state.getValue()).toBeNull();
    expect(localStorage.getItem('ShopMateSession')).toBeNull();
  });

  it('rejects expired or incomplete sessions', () => {
    const state = new StateService();
    localStorage.setItem('ShopMateSession', JSON.stringify({ accessToken: 'token' }));
    expect(state.getValue()).toBeNull();

    localStorage.setItem('ShopMateSession', JSON.stringify({
      accessToken: 'expired-token',
      expiresAtUtc: new Date(Date.now() - 60_000).toISOString(),
      user: { id: 1, fullName: 'Class Learner', username: 'learner', role: 'User' },
    }));
    expect(state.getValue()).toBeNull();
    expect(localStorage.getItem('ShopMateSession')).toBeNull();
  });

  it('restores a valid stored session before route guards run', () => {
    const stored = {
      accessToken: 'saved-token',
      expiresAtUtc: new Date(Date.now() + 60_000).toISOString(),
      user: { id: 1, fullName: 'Class Learner', username: 'learner', role: 'User' },
    };
    localStorage.setItem('ShopMateSession', JSON.stringify(stored));
    TestBed.configureTestingModule({
      providers: [AuthService, ShoppingListService, StateService, provideRouter([])],
    });

    const auth = TestBed.inject(AuthService);
    expect(auth.isAuthenticated()).toBe(true);
    expect(auth.getToken()).toBe('saved-token');
  });
});
