import { Component, inject } from '@angular/core';
import { ShoppingListService } from '../../services/shopping-list';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  imports: [RouterLink, RouterLinkActive],
  selector: 'app-shop-header',
  templateUrl: './shop-header.html',
})
export class ShopHeader {
  readonly shoppingListService = inject(ShoppingListService);
}
