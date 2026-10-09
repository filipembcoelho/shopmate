import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ShoppingLists } from './shopping-lists';

describe('ShoppingLists', () => {
  let component: ShoppingLists;
  let fixture: ComponentFixture<ShoppingLists>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShoppingLists],
    }).compileComponents();

    fixture = TestBed.createComponent(ShoppingLists);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
