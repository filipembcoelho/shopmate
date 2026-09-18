import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ShopHeader } from './shop-header';

describe('ShopHeader', () => {
  let component: ShopHeader;
  let fixture: ComponentFixture<ShopHeader>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShopHeader],
    }).compileComponents();

    fixture = TestBed.createComponent(ShopHeader);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
