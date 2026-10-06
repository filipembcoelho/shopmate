import { Component, inject } from '@angular/core';
import { ShopHeader } from './components/shop-header/shop-header';
import { ShopFooter } from './components/shop-footer/shop-footer';
import { ShoppingListService } from './services/shopping-list';
import { ShoppingList } from './components/shopping-list/shopping-list';
import { SessionDemoService } from './services/session-demo';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  imports: [ShopHeader, ShopFooter, ShoppingList],
})
export class App {
  readonly listService = inject(ShoppingListService);
  readonly sessionDemoService = inject(SessionDemoService);

  togglePurchased(event: { listId: number; itemId: number }): void {
    this.listService.togglePurchased(event);
  }

  runSessionDemo() {
    this.sessionDemoService.run();
  }
}
