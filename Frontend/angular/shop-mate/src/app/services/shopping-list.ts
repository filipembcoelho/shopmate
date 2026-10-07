import { HttpClient } from '@angular/common/http';
import { computed, inject, Service, signal } from '@angular/core';
import { ShoppingListData } from '../Model/ShoppingListData';

@Service()
export class ShoppingListService {
  private readonly url = 'http://localhost:3000/shoppingLists';
  private readonly http = inject(HttpClient);

  readonly currentLists = signal<ShoppingListData[]>([]);
  readonly hasLoaded = signal(false);
  readonly isCreating = signal(false);
  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  readonly remainingCount = computed(() =>
    this.currentLists().reduce(
      (total, list) => total + list.items.filter((item) => !item.purchased).length,
      0,
    ),
  );

  getLists(): void {
    this.errorMessage.set('');

    this.http.get<ShoppingListData[]>(this.url).subscribe({
      next: (lists) => {
        this.currentLists.set(lists);
        this.hasLoaded.set(true);
      },
      error: (error) => {
        console.error('Could not load shopping lists', error);
        this.errorMessage.set('Could not load the lists. Check that JSON Server is running.');
      },
    });
  }

  togglePurchased(event: { listId: string; itemId: number }): void {
    this.currentLists.update((lists) => {
      for (const list of lists) {
        if (list.id !== event.listId) continue;

        for (const item of list.items) {
          if (item.id !== event.itemId) continue;

          item.purchased = !item.purchased;
          return lists.slice();
        }
      }

      return lists;
    });
  }

  createList(title: string): void {
    if (this.isCreating()) return;

    this.clearFeedback();
    this.isCreating.set(true);

    this.http.get<ShoppingListData[]>(this.url).subscribe({
      next: (lists) => {
        this.saveNewList(title.trim(), this.nextListId(lists));
      },
      error: (error) => {
        console.error('Could not check shopping list IDs', error);
        this.isCreating.set(false);
        this.errorMessage.set('Could not check the existing lists. Please try again.');
      },
    });
  }

  private nextListId(lists: ShoppingListData[]): string {
    let highestId = 0;

    for (const list of lists) {
      const id = Number(list.id);
      if (Number.isInteger(id) && id > highestId) {
        highestId = id;
      }
    }

    return String(highestId + 1);
  }

  private saveNewList(title: string, id: string): void {
    const newList: ShoppingListData = { id, title, items: [] };

    this.http.post<ShoppingListData>(this.url, newList).subscribe({
      next: () => {
        this.isCreating.set(false);
        this.successMessage.set('Shopping list created.');
        this.getLists();
      },
      error: (error) => {
        console.error('Could not create shopping list', error);
        this.isCreating.set(false);
        this.errorMessage.set('Could not create the list. Please try again.');
      },
    });
  }

  renameList(listId: string, newTitle: string): void {
    const list = this.currentLists().find((current) => current.id === listId);
    if (!list) {
      this.errorMessage.set('Choose a list that is still on the page.');
      return;
    }

    this.clearFeedback();

    const updatedList: ShoppingListData = {
      id: list.id,
      title: newTitle.trim(),
      items: list.items,
    };

    this.http.put<ShoppingListData>(this.url + '/' + listId, updatedList).subscribe({
      next: () => {
        this.successMessage.set('Shopping list renamed.');
        this.getLists();
      },
      error: (error) => {
        console.error('Could not rename shopping list', error);
        this.errorMessage.set('Could not rename the list. Please try again.');
      },
    });
  }

  deleteList(listId: string): void {
    const list = this.currentLists().find((current) => current.id === listId);
    if (!list) {
      this.errorMessage.set('Choose a list that is still on the page.');
      return;
    }

    this.clearFeedback();

    this.http.delete<void>(this.url + '/' + listId).subscribe({
      next: () => {
        this.successMessage.set('Shopping list deleted.');
        this.getLists();
      },
      error: (error) => {
        console.error('Could not delete shopping list', error);
        this.errorMessage.set('Could not delete the list. Please try again.');
      },
    });
  }

  private clearFeedback(): void {
    this.errorMessage.set('');
    this.successMessage.set('');
  }
}
