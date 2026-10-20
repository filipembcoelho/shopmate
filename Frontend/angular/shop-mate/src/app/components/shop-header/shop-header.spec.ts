import { Component } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { AuthService } from '../../services/auth';
import { ShoppingListService } from '../../services/shopping-list';
import { StateService } from '../../services/state-service';
import { ShopHeader } from './shop-header';

@Component({ template: '' })
class EmptyLoginPage {}

describe('ShopHeader', () => {
  let component: ShopHeader;
  let fixture: ComponentFixture<ShopHeader>;

  beforeEach(async () => {
    localStorage.clear();
    await TestBed.configureTestingModule({
      imports: [ShopHeader],
      providers: [
        AuthService,
        ShoppingListService,
        StateService,
        provideRouter([{ path: 'login', component: EmptyLoginPage }]),
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ShopHeader);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('shows the signed-in user and offers sign-out instead of public links', () => {
    const auth = TestBed.inject(AuthService);
    auth.login('learner', 'Classroom1!').subscribe();
    TestBed.inject(HttpTestingController)
      .expectOne('https://localhost:7027/api/auth/login')
      .flush({
        accessToken: 'classroom-token',
        expiresAtUtc: new Date(Date.now() + 60_000).toISOString(),
        user: { id: 1, fullName: 'Class Learner', username: 'learner', role: 'User' },
      });
    fixture.detectChanges();

    const navigation = fixture.nativeElement.querySelector('nav') as HTMLElement;
    expect(navigation.textContent).toContain('Class Learner');
    expect(navigation.textContent).toContain('Sign out');
    expect(navigation.textContent).not.toContain('Create account');

    navigation.querySelector('button')?.click();
    fixture.detectChanges();
    expect(navigation.textContent).toContain('Sign in');
    expect(localStorage.getItem('ShopMateSession')).toBeNull();
  });
});
