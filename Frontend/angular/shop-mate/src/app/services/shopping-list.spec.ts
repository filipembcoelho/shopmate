import { TestBed } from '@angular/core/testing';
import { ShoppingList } from './shopping-list';

describe('ShoppingList', () => {
  let service: ShoppingList;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ShoppingList);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
