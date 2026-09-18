import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ShopFooter } from './shop-footer';

describe('ShopFooter', () => {
  let component: ShopFooter;
  let fixture: ComponentFixture<ShopFooter>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShopFooter],
    }).compileComponents();

    fixture = TestBed.createComponent(ShopFooter);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
