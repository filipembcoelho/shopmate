import { computed, inject, Service, signal } from '@angular/core';
import { ShoppingListData } from '../Model/ShoppingListData';
import { createSeedShoppingLists } from '../data/shopping-list.seed';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';

@Service()
export class ShoppingListService {
  readonly http = inject(HttpClient);
  readonly currentLists = signal<ShoppingListData[]>([]);

  // readonly lists = this.currentLists.asReadonly();
  // private readonly currentLists = signal<ShoppingListData[]>(createSeedShoppingLists());

  readonly remainingCount = computed(() =>
    this.currentLists().reduce(
      (total, list) => total + list.items.filter((x) => !x.purchased).length,
      0,
    ),
  );

  togglePurchased(event: { listId: number; itemId: number }) {
    //   this.currentLists.update((lists) =>
    //     lists.map((list) => {
    //       // if (list.id !== event.listId) return list;
    //       const foundItem = list.items.find((item) => item.id === event.itemId);
    //       if (foundItem) {
    //         foundItem.purchased = !foundItem.purchased;
    //       }
    //       return list;
    //     }),
    //   );
  }

  method() {
    this.http.get<ShoppingListData[]>('http://localhost:3000/shoppingLists').subscribe({
      next: (apiLists) => {
        console.log('apiLists', apiLists);

        const loadedLists: ShoppingListData[] = [];

        for (const apiList of apiLists) {
          loadedLists.push({
            id: Number(apiList.id),
            title: apiList.title,
            items: apiList.items,
          });
        }

        this.currentLists.set(loadedLists);
      },
      error: (error) => {
        console.error('Error fetching shopping lists:', error);
      },
    });
  }

  loadFromMockJsonServer() {
    this.http.get<ShoppingListData[]>('http://localhost:3000/shoppingLists').subscribe({
      next: (apiLists) => {
        console.log('apiLists', apiLists);
      },
      error: (error) => {
        console.error('Error fetching shopping lists:', error);
      },
    });
  }
}
