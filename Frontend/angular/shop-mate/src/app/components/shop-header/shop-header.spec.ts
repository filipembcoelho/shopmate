import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { ShopHeader } from './shop-header';

describe('ShopHeader', () => {
  let component: ShopHeader;
  let fixture: ComponentFixture<ShopHeader>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShopHeader],
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(ShopHeader);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
