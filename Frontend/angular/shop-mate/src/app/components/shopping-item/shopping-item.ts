import { Component, input, output, signal } from '@angular/core';
import { ShoppingListItem } from '../../Model/ShoppingListItem';
import { TitleCasePipe } from '@angular/common';
import { QuantityLabelPipe } from '../../pipes/quantity-label-pipe';

@Component({
  imports: [TitleCasePipe, QuantityLabelPipe],
  selector: 'app-shopping-item',
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
