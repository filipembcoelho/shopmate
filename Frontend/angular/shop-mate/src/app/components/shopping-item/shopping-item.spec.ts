import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ShoppingItem } from './shopping-item';

describe('ShoppingItem', () => {
  let component: ShoppingItem;
  let fixture: ComponentFixture<ShoppingItem>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShoppingItem],
    }).compileComponents();

    fixture = TestBed.createComponent(ShoppingItem);
    fixture.componentRef.setInput('shoppingListItemInner', {
      id: 7,
      name: 'Massa',
      quantity: 2,
      purchased: false,
    });
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('emits its item ID when status is requested', () => {
    const emitted: number[] = [];
    component.toggleRequested.subscribe((id) => emitted.push(id));

    component.requestToggle();

    expect(emitted).toEqual([7]);
  });
});
