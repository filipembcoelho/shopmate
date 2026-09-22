import { Component, input, output } from '@angular/core';
import { ShoppingListData } from '../../Model/ShoppingListData';
import { ShoppingItem } from '../shopping-item/shopping-item';

@Component({
  imports: [ShoppingItem],
  selector: 'app-shopping-list',
  styleUrl: './shopping-list.css',
  templateUrl: './shopping-list.html',
})
export class ShoppingList {
  shoppingList = input.required<ShoppingListData>();

  toggleRequested = output<{ listId: number; itemId: number }>();

  requestToggle(itemId: number) {
    const listId = this.shoppingList().id;

    this.toggleRequested.emit({
      listId,
      itemId,
    });
  }
}
