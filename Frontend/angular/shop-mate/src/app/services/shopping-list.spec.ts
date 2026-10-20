import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { ShoppingListService } from './shopping-list';

describe('ShoppingListService item updates', () => {
  let service: ShoppingListService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ShoppingListService, provideHttpClient(), provideHttpClientTesting()],
    });

    service = TestBed.inject(ShoppingListService);
    http = TestBed.inject(HttpTestingController);
    service.currentLists.set([
      {
        id: '3',
        title: 'Weekly shop',
        items: [{ id: 4, name: 'Milk', quantity: 2, unit: 1, purchased: false }],
      },
    ]);
  });

  afterEach(() => http.verify({ ignoreCancelled: true }));

  it('saves a purchase change before showing it and can reopen the item', () => {
    service.togglePurchased({ listId: '3', itemId: 4 });
    service.togglePurchased({ listId: '3', itemId: 4 });

    expect(service.currentLists()[0].items[0].purchased).toBe(false);

    const complete = http.expectOne('https://localhost:7027/api/shopping-lists/3/items/4');
    expect(complete.request.method).toBe('PUT');
    expect(complete.request.body).toEqual({
      name: 'Milk',
      quantity: 2,
      unit: 1,
      isCompleted: true,
    });
    complete.flush({ id: 4, name: 'Milk', quantity: 2, unit: 1, isCompleted: true });

    expect(service.currentLists()[0].items[0].purchased).toBe(true);

    service.togglePurchased({ listId: '3', itemId: 4 });

    const reopen = http.expectOne('https://localhost:7027/api/shopping-lists/3/items/4');
    expect(reopen.request.body.isCompleted).toBe(false);
    reopen.flush({ id: 4, name: 'Milk', quantity: 2, unit: 1, isCompleted: false });

    expect(service.currentLists()[0].items[0].purchased).toBe(false);
  });

  it('leaves the item unchanged and shows an error when saving fails', () => {
    service.togglePurchased({ listId: '3', itemId: 4 });

    const request = http.expectOne('https://localhost:7027/api/shopping-lists/3/items/4');
    request.flush({ message: 'Server error' }, { status: 500, statusText: 'Server error' });

    expect(service.currentLists()[0].items[0].purchased).toBe(false);
    expect(service.errorMessage()).toContain('Could not update');
  });

  it('discards an old list response after the session is cleared', () => {
    service.getLists();
    const request = http.expectOne('https://localhost:7027/api/shopping-lists');

    service.reset();

    expect(request.cancelled).toBe(true);
    expect(service.currentLists()).toEqual([]);
    expect(service.hasLoaded()).toBe(false);
  });
});
