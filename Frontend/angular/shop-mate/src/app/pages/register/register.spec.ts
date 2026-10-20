import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Register } from './register';

describe('Register', () => {
  it('shows the API validation message when account creation fails', async () => {
    await TestBed.configureTestingModule({
      imports: [Register],
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();
    const fixture = TestBed.createComponent(Register);
    const component = fixture.componentInstance;
    component.form.setValue({
      fullName: 'Class Learner',
      username: 'learner',
      password: 'Classroom1!',
    });

    component.submit();
    TestBed.inject(HttpTestingController)
      .expectOne('https://localhost:7027/api/auth/register')
      .flush({ message: 'That username is already taken.' }, {
        status: 400,
        statusText: 'Bad Request',
      });

    expect(component.errorMessage()).toBe('That username is already taken.');
    expect(component.isSubmitting()).toBe(false);
    TestBed.inject(HttpTestingController).verify();
  });
});
