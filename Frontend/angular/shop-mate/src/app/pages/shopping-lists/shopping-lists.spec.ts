import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { ShoppingLists } from './shopping-lists';

describe('ShoppingLists', () => {
  let component: ShoppingLists;
  let fixture: ComponentFixture<ShoppingLists>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShoppingLists],
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(ShoppingLists);
    component = fixture.componentInstance;
    await fixture.whenStable();
    TestBed.inject(HttpTestingController)
      .expectOne('https://localhost:7027/api/shopping-lists')
      .flush([]);
  });

  afterEach(() => TestBed.inject(HttpTestingController).verify());

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
