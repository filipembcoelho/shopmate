import { Component, inject } from '@angular/core';
import { ShoppingListService } from '../../services/shopping-list';

@Component({
  imports: [],
  selector: 'app-shop-header',
  templateUrl: './shop-header.html',
})
export class ShopHeader {
  readonly shoppingListService = inject(ShoppingListService);
}
