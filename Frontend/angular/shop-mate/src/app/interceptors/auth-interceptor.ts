import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth';
import { catchError, throwError } from 'rxjs';
import { API_BASE_URL } from '../services/api-config';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const auth = inject(AuthService);

  const api = new URL(API_BASE_URL);
  const target = new URL(request.url, window.location.origin);
  const isOurApi = target.origin === api.origin &&
    target.pathname.startsWith(api.pathname + '/');

  const isPublicAuth =
    target.pathname === api.pathname + '/auth/login' ||
    target.pathname === api.pathname + '/auth/register';

  if (!isOurApi || isPublicAuth) {
    return next(request);
  }

  const token = auth.getToken();

  const outgoing = token
    ? request.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : request;

  return next(outgoing).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && token !== null && auth.getToken() === token) {
        auth.logout();
      }

      return throwError(() => error);
    }),
  );
};
