import { HttpClient } from '@angular/common/http';
import { computed, inject, Service, signal } from '@angular/core';
import { ShoppingListApiDto, ShoppingListData } from '../Model/ShoppingListData';
import { ShoppingListItem, ShoppingListItemApiDto } from '../Model/ShoppingListItem';
import { map, Observable } from 'rxjs';

@Service()
export class ShoppingListService {
  private readonly url = 'https://localhost:7027/api/shopping-lists';

  private readonly http = inject(HttpClient);

  readonly currentLists = signal<ShoppingListData[]>([]);
  readonly hasLoaded = signal(false);
  readonly isCreating = signal(false);
  readonly isRenaming = signal(false);
  readonly isAddingItem = signal(false);
  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  private readonly demoUserId = 1;

  readonly remainingCount = computed(() =>
    this.currentLists().reduce(
      (total, list) => total + list.items.filter((item) => !item.purchased).length,
      0,
    ),
  );

  getLists(): void {
    this.errorMessage.set('');

    const url = `${this.url}?userId=${this.demoUserId}`;

    this.http.get<ShoppingListApiDto[]>(url).subscribe({
      next: (lists) => {
        const convertedLists: ShoppingListData[] = [];

        for (const list of lists) {
          convertedLists.push(this.fromApi(list));
        }

        this.currentLists.set(convertedLists);
        this.hasLoaded.set(true);
      },
      error: (error) => {
        console.error('Could not load shopping lists', error);
        this.errorMessage.set('Could not load the lists. Check that the API is running.');
      },
    });
  }

  getList(listId: string): Observable<ShoppingListData> {
    const url = `${this.url}/${listId}?userId=${this.demoUserId}`;

    return this.http.get<ShoppingListApiDto>(url).pipe(map((list) => this.fromApi(list)));
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

  createList(title: string, onSaved: () => void): void {
    if (this.isCreating()) return;

    this.clearFeedback();
    this.isCreating.set(true);

    this.http
      .post<ShoppingListApiDto>(this.url, {
        name: title.trim(),
        ownerId: this.demoUserId,
      })
      .subscribe({
        next: () => {
          this.isCreating.set(false);
          this.successMessage.set('Shopping list created.');
          onSaved();
        },
        error: (error) => {
          console.error('Could not create shopping list', error);
          this.isCreating.set(false);
          this.errorMessage.set('Could not create the list. Please try again.');
        },
      });
  }

  renameList(listId: string, newTitle: string, onSaved: () => void): void {
    if (this.isRenaming()) return;
    this.clearFeedback();
    this.isRenaming.set(true);

    const url = `${this.url}/${listId}?userId=${this.demoUserId}`;

    this.http
      .put<ShoppingListApiDto>(url, {
        name: newTitle.trim(),
      })
      .subscribe({
        next: () => {
          this.isRenaming.set(false);
          this.successMessage.set('Shopping list renamed.');
          onSaved();
        },
        error: (error) => {
          console.error('Could not rename shopping list', error);
          this.isRenaming.set(false);
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

    const url = `${this.url}/${listId}?userId=${this.demoUserId}`;

    this.http.delete<void>(url).subscribe({
      next: () => {
        this.successMessage.set('Shopping list archived.');
        this.getLists();
      },
      error: (error) => {
        console.error('Could not archive shopping list', error);
        this.errorMessage.set('Could not archive the list. Please try again.');
      },
    });
  }

  addItem(
    listId: string,
    name: string,
    quantity: number,
    unit: number,
    onSaved: () => void,
  ): void {
    if (this.isAddingItem()) return;

    this.clearFeedback();
    this.isAddingItem.set(true);

    const url = `${this.url}/${listId}/items?userId=${this.demoUserId}`;

    this.http
      .post<ShoppingListItemApiDto>(url, {
        name: name.trim(),
        quantity,
        unit,
      })
      .subscribe({
        next: () => {
          this.isAddingItem.set(false);
          this.successMessage.set('Item added.');
          this.getLists();
          onSaved();
        },
        error: (error) => {
          console.error('Could not add item', error);
          this.isAddingItem.set(false);
          this.errorMessage.set('Could not add the item. Please try again.');
        },
      });
  }

  private clearFeedback(): void {
    this.errorMessage.set('');
    this.successMessage.set('');
  }

  private fromApi(list: ShoppingListApiDto): ShoppingListData {
    const items: ShoppingListItem[] = [];

    const id = String(list.id);
    const title = list.name;

    for (const item of list.items) {
      items.push({
        id: item.id,
        name: item.name,
        quantity: item.quantity,
        unit: item.unit,
        purchased: item.isCompleted,
      });
    }

    return { id, title, items };
  }
}
