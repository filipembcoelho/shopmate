import { Component, inject } from '@angular/core';
import { ShopHeader } from './components/shop-header/shop-header';
import { ShopFooter } from './components/shop-footer/shop-footer';
import { ShoppingListService } from './services/shopping-list';
import { ShoppingList } from './components/shopping-list/shopping-list';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  imports: [ShopHeader, ShopFooter, ShoppingList],
})
export class App {
  readonly listService = inject(ShoppingListService);

  togglePurchased(event: { listId: number; itemId: number }): void {
    this.listService.togglePurchased(event);
  }
}
