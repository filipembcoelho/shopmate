import { computed, inject, Service, signal } from '@angular/core';
import { ShoppingListData } from '../Model/ShoppingListData';
import { createSeedShoppingLists } from '../data/shopping-list.seed';
import { HttpClient } from '@angular/common/http';

@Service()
export class ShoppingListService {
  readonly http = inject(HttpClient);

  private readonly currentLists = signal<ShoppingListData[]>(createSeedShoppingLists());

  readonly lists = this.currentLists.asReadonly();

  readonly remainingCount = computed(() =>
    this.currentLists().reduce(
      (total, list) => total + list.items.filter((x) => !x.purchased).length,
      0,
    ),
  );

  togglePurchased(event: { listId: number; itemId: number }) {
    this.currentLists.update((lists) =>
      lists.map((list) => {
        if (list.id !== event.listId) return list;

        const foundItem = list.items.find((item) => item.id === event.itemId);

        if (foundItem) {
          foundItem.purchased = !foundItem.purchased;
        }

        return list;
      }),
    );
  }

  method() {
    this.http.get<ShoppingListData[]>('http://localhost:4897/shoppinglists').subscribe();
  }
}
