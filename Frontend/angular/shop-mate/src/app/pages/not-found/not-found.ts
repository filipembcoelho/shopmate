import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-not-found',
  styleUrl: './not-found.css',
  templateUrl: './not-found.html',
})
export class NotFound implements OnInit, OnDestroy {
  private readonly router = inject(Router);
  private intervalId: ReturnType<typeof setInterval> | undefined;

  readonly secondsRemaining = signal(10);

  ngOnInit(): void {
    this.intervalId = setInterval(() => {
      const remaining = this.secondsRemaining() - 1;
      this.secondsRemaining.set(remaining);

      if (remaining === 0) {
        this.stopCountdown();
        this.goHome();
      }
    }, 1000);
  }

  ngOnDestroy(): void {
    this.stopCountdown();
  }

  goHome(): void {
    this.router.navigateByUrl('/');
  }

  private stopCountdown(): void {
    if (this.intervalId !== undefined) {
      clearInterval(this.intervalId);
      this.intervalId = undefined;
    }
  }
}
