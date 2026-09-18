import { Component, input, output, signal } from '@angular/core';
import { ShoppingListItem } from '../../Model/ShoppingListItem';

@Component({
  imports: [],
  selector: 'app-shopping-item',
  styleUrl: './shopping-item.css',
  templateUrl: './shopping-item.html',
})
export class ShoppingItem {
  showDetails = signal(false);

  shoppingListItemInner = input.required<ShoppingListItem>();

  toggleRequested = output<number>();

  toggleDetails() {
    this.showDetails.update((currentValue) => !currentValue);
  }

  requestToggle() {
    this.toggleRequested.emit(this.shoppingListItemInner().id);
  }
}
