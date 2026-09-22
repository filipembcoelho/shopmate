import { Component, input, output } from '@angular/core';
import { ShoppingListData } from '../../Model/ShoppingListData';
import { ShoppingItem } from '../shopping-item/shopping-item';
import { QuantityLabelPipe } from '../../pipes/quantity-label-pipe';

@Component({
  imports: [ShoppingItem, QuantityLabelPipe],
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

  method() {
    let a = 1;
    switch (a) {
      case 1:
        console.log(a);
        break;
      default:
        console.log(a + 1);
        break;
    }
  }
}
