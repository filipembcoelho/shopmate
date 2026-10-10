import { Component, input, output } from '@angular/core';
import { ShoppingListData } from '../../Model/ShoppingListData';
import { ShoppingItem } from '../shopping-item/shopping-item';
import { AddItemForm } from '../add-item-form/add-item-form';
import { RouterLink } from '@angular/router';

@Component({
  imports: [ShoppingItem, AddItemForm, RouterLink],
  selector: 'app-shopping-list',
  templateUrl: './shopping-list.html',
})
export class ShoppingList {
  shoppingList = input.required<ShoppingListData>();

  toggleRequested = output<{ listId: string; itemId: number }>();
  deleteRequested = output<string>();

  requestToggle(itemId: number) {
    const listId = this.shoppingList().id;

    this.toggleRequested.emit({
      listId,
      itemId,
    });
  }
}
