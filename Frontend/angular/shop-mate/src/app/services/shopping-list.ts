import { HttpClient } from '@angular/common/http';
import { computed, inject, Service, signal } from '@angular/core';
import { ShoppingListApiDto, ShoppingListData } from '../Model/ShoppingListData';
import { ShoppingListItem, ShoppingListItemApiDto } from '../Model/ShoppingListItem';
import { map, Observable, Subject, takeUntil } from 'rxjs';
import { API_BASE_URL } from './api-config';

@Service()
export class ShoppingListService {
  private readonly url = `${API_BASE_URL}/shopping-lists`;

  private readonly http = inject(HttpClient);
  private readonly sessionEnded = new Subject<void>();

  readonly currentLists = signal<ShoppingListData[]>([]);
  readonly hasLoaded = signal(false);
  readonly isCreating = signal(false);
  readonly isRenaming = signal(false);
  readonly isAddingItem = signal(false);
  readonly isUpdatingItem = signal(false);
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

    this.http.get<ShoppingListApiDto[]>(this.url).pipe(takeUntil(this.sessionEnded)).subscribe({
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
    const url = `${this.url}/${listId}`;

    return this.http.get<ShoppingListApiDto>(url).pipe(
      takeUntil(this.sessionEnded),
      map((list) => this.fromApi(list)),
    );
  }

  togglePurchased(event: { listId: string; itemId: number }): void {
    if (this.isUpdatingItem()) return;

    const list = this.currentLists().find((current) => current.id === event.listId);
    const item = list?.items.find((current) => current.id === event.itemId);

    if (!item || item.unit === undefined) {
      this.errorMessage.set('Could not update this item. Reload the lists and try again.');
      return;
    }

    this.clearFeedback();
    this.isUpdatingItem.set(true);

    const url = `${this.url}/${event.listId}/items/${event.itemId}`;

    this.http
      .put<ShoppingListItemApiDto>(url, {
        name: item.name,
        quantity: item.quantity,
        unit: item.unit,
        isCompleted: !item.purchased,
      })
      .pipe(takeUntil(this.sessionEnded))
      .subscribe({
        next: (updatedItem) => {
          this.currentLists.update((lists) => {
            for (const currentList of lists) {
              if (currentList.id !== event.listId) continue;

              for (const currentItem of currentList.items) {
                if (currentItem.id !== event.itemId) continue;

                currentItem.purchased = updatedItem.isCompleted;
                return lists.slice();
              }
            }

            return lists;
          });
          this.isUpdatingItem.set(false);
        },
        error: (error) => {
          console.error('Could not update shopping list item', error);
          this.isUpdatingItem.set(false);
          this.errorMessage.set('Could not update the item. Please try again.');
        },
      });
  }

  createList(title: string, onSaved: () => void): void {
    if (this.isCreating()) return;

    this.clearFeedback();
    this.isCreating.set(true);

    this.http
      .post<ShoppingListApiDto>(this.url, {
        name: title.trim(),
      })
      .pipe(takeUntil(this.sessionEnded))
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

    const url = `${this.url}/${listId}`;

    this.http
      .put<ShoppingListApiDto>(url, {
        name: newTitle.trim(),
      })
      .pipe(takeUntil(this.sessionEnded))
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

    const url = `${this.url}/${listId}`;

    this.http.delete<void>(url).pipe(takeUntil(this.sessionEnded)).subscribe({
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

  addItem(listId: string, name: string, quantity: number, unit: number, onSaved: () => void): void {
    if (this.isAddingItem()) return;

    this.clearFeedback();
    this.isAddingItem.set(true);

    const url = `${this.url}/${listId}/items`;

    this.http
      .post<ShoppingListItemApiDto>(url, {
        name: name.trim(),
        quantity,
        unit,
      })
      .pipe(takeUntil(this.sessionEnded))
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

  reset(): void {
    this.sessionEnded.next();
    this.currentLists.set([]);
    this.hasLoaded.set(false);
    this.isCreating.set(false);
    this.isRenaming.set(false);
    this.isAddingItem.set(false);
    this.isUpdatingItem.set(false);
    this.clearFeedback();
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
