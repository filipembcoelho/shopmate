import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
})
export class Register {
  private readonly formBuilder = inject(FormBuilder);
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
    // Session 10.1: replace this body with the real registration request.
    this.errorMessage.set('Account creation is not available yet.');
  }
}
