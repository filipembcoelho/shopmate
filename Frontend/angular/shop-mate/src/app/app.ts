import { Component } from '@angular/core';
import { ShopHeader } from './components/shop-header/shop-header';
import { ShoppingItem } from './components/shopping-item/shopping-item';
import { ShoppingListItem } from './Model/ShoppingListItem';

@Component({
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
  imports: [ShopHeader, ShoppingItem],
})
export class App {
  title: string = 'shop-mate-v2';
  items: ShoppingListItem[] = [];

  constructor() {
    this.items = [
      {
        id: 1,
        name: 'Arroz',
        purchased: false,
        quantity: 3,
      },
      {
        id: 2,
        name: 'Massa',
        purchased: false,
        quantity: 2,
      },
      {
        id: 3,
        name: 'Leite',
        purchased: false,
        quantity: 2,
      },
    ];
  } // services

  // TODO: talk about oninit

  addItems(item: ShoppingListItem) {
    if (item.name.trim() === '' || item.quantity <= 0) {
      return;
    }
    this.items.push(item);
    // add to the API
  }

  togglePurchased(id: number) {
    const foundItem = this.items.find((i) => i.id == id);

    if (foundItem) {
      foundItem.purchased = !foundItem.purchased;
    }
  }
}
