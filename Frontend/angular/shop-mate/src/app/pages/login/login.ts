import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
})
export class Login {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly registered = inject(ActivatedRoute).snapshot.queryParamMap.get('registered') === 'true';
  readonly isSubmitting = signal(false);
  readonly errorMessage = signal('');

  readonly form = this.formBuilder.nonNullable.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
  });

  submit(): void {
    this.form.markAllAsTouched();
    if (this.form.invalid || this.isSubmitting()) return;

    this.isSubmitting.set(true);

    this.errorMessage.set('');

    const values = this.form.getRawValue();

    this.authService.login(values.username, values.password).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.form.controls.password.reset();
        void this.router.navigateByUrl('/');
      },
      error: (error) => {
        this.isSubmitting.set(false);
        this.errorMessage.set(
          error.status === 401
            ? 'Invalid username or password.'
            : 'Could not sign in. Check the API and try again.',
        );
      },
    });
  }
}
