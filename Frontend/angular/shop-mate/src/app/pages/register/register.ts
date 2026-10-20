import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
})
export class Register {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly isSubmitting = signal(false);
  readonly errorMessage = signal('');

  readonly form = this.formBuilder.nonNullable.group({
    fullName: ['', Validators.required],
    username: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(8)]],
  });

  submit(): void {
    this.form.markAllAsTouched();
    if (this.form.invalid || this.isSubmitting()) return;

    this.isSubmitting.set(true);

    this.errorMessage.set('');

    const values = this.form.getRawValue();

    this.authService.register(values.fullName, values.username, values.password).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.form.reset();
        void this.router.navigate(['/login'], {
          queryParams: { registered: true },
        });
      },
      error: (error) => {
        this.isSubmitting.set(false);
        this.errorMessage.set(
          error.status === 400
            ? error.error?.message ?? 'Check your registration details.'
            : 'Could not create the account. Try again.',
        );
      },
    });
  }
}
