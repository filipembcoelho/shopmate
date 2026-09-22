import { Component, ɵallLeavingAnimations } from '@angular/core';
import { ShopHeader } from './components/shop-header/shop-header';
import { ShopFooter } from './components/shop-footer/shop-footer';
import { ShoppingListData } from './Model/ShoppingListData';
import { createSeedShoppingLists } from './data/shopping-list.seed';
import { ShoppingList } from './components/shopping-list/shopping-list';

@Component({
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
  imports: [ShopHeader, ShopFooter, ShoppingList],
})
export class App {
  lists: ShoppingListData[] = createSeedShoppingLists();

  constructor() {}

  togglePurchased(event: { listId: number; itemId: number }) {
    const list = this.lists.find((c) => c.id === event.listId);

    if (!list) {
      return;
    }

    const foundItem = list.items.find((i) => i.id == event.itemId);

    if (foundItem) {
      foundItem.purchased = !foundItem.purchased;
    }
  }
}
