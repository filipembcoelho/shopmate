import { Component, input, output, signal } from '@angular/core';
import { ShoppingListItem } from '../../Model/ShoppingListItem';
import { PercentPipe, TitleCasePipe } from '@angular/common';

@Component({
  imports: [TitleCasePipe, PercentPipe],
  selector: 'app-shopping-item',
  styleUrl: './shopping-item.css',
  templateUrl: './shopping-item.html',
})
export class ShoppingItem {
  showDetails = signal(false);

  itemPrice: number = 0.05;

  shoppingListItemInner = input.required<ShoppingListItem>();

  toggleRequested = output<number>();

  toggleDetails() {
    this.showDetails.update((currentValue) => !currentValue);
  }

  requestToggle() {
    this.toggleRequested.emit(this.shoppingListItemInner().id);
  }
}
