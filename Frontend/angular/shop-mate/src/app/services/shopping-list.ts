import { computed, inject, Service, signal } from '@angular/core';
import { ShoppingListData } from '../Model/ShoppingListData';
import { HttpClient } from '@angular/common/http';

@Service()
export class ShoppingListService {
  private readonly host = 'http://localhost:3000';
  private readonly controller = 'shoppingLists';
  private readonly url = `${this.host}/${this.controller}`;

  readonly http = inject(HttpClient);
  readonly currentLists = signal<ShoppingListData[]>([]);

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

  getLists() {
    this.http.get<ShoppingListData[]>(this.url).subscribe({
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

  createList(title: string) {
    const newList = {
      title: title,
      items: [],
    };

    this.http.post<ShoppingListData>(this.url, newList).subscribe({
      next: (createdList) => {
        console.log('Created new list:', createdList);
        this.getLists();
      },
      error: (error) => {
        console.error('Error creating shopping list:', error);
      },
    });
  }

  renameList(listId: number, newTitle: string) {
    const list = this.currentLists().find((l) => l.id === listId);

    if (!list) {
      console.error(`List with ID ${listId} not found.`);
      return;
    }

    const updatedList = {
      id: list.id,
      title: newTitle,
      items: list.items,
    };

    this.http.put<ShoppingListData>(`${this.url}/${listId}`, updatedList).subscribe({
      next: (response) => {
        console.log('Renamed list:', response);
        this.getLists();
      },
      error: (error) => {
        console.error('Error renaming shopping list:', error);
      },
    });
  }

  deleteList(listId: number) {
    this.http.delete(`${this.url}/${listId}`).subscribe({
      next: () => {
        console.log(`Deleted list with ID ${listId}`);
        this.getLists();
      },
      error: (error) => {
        console.error(`Error deleting shopping list with ID ${listId}:`, error);
      },
    });
  }
}
