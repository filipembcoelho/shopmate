import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
})
export class Login {
  private readonly formBuilder = inject(FormBuilder);
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
    // Session 10.1: replace this body with the real login request and navigation.
    this.errorMessage.set('Sign-in is not available yet.');
  }
}
