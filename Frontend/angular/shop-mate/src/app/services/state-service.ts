import { Service } from '@angular/core';
import { LoginResponse } from '../Model/AuthData';

@Service()
export class StateService {
  private readonly key = 'ShopMateSession';

  setValue(session: LoginResponse): void {
    localStorage.setItem(this.key, JSON.stringify(session));
  }

  getValue(): LoginResponse | null {
    const value = localStorage.getItem(this.key);
    if (value === null) return null;

    try {
      const session: unknown = JSON.parse(value);
      if (this.isLoginResponse(session) && Date.parse(session.expiresAtUtc) > Date.now()) {
        return session;
      }
    } catch {
      // A broken stored value is treated as a signed-out session.
    }

    this.deleteValue();
    return null;
  }

  deleteValue(): void {
    localStorage.removeItem(this.key);
  }

  private isLoginResponse(value: unknown): value is LoginResponse {
    if (typeof value !== 'object' || value === null || Array.isArray(value)) return false;

    const session = value as Record<string, unknown>;
    if (typeof session['accessToken'] !== 'string' || session['accessToken'].length === 0 ||
        typeof session['expiresAtUtc'] !== 'string') return false;

    const user = session['user'];
    if (typeof user !== 'object' || user === null || Array.isArray(user)) return false;

    const account = user as Record<string, unknown>;
    return typeof account['id'] === 'number' && Number.isInteger(account['id']) && account['id'] > 0 &&
      typeof account['fullName'] === 'string' &&
      typeof account['username'] === 'string' &&
      typeof account['role'] === 'string';
  }
}
