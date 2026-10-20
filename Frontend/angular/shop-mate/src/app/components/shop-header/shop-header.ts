import { Component, inject } from '@angular/core';
import { ShoppingListService } from '../../services/shopping-list';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  imports: [RouterLink, RouterLinkActive],
  selector: 'app-shop-header',
  templateUrl: './shop-header.html',
})
export class ShopHeader {
  readonly shoppingListService = inject(ShoppingListService);
  readonly auth = inject(AuthService);
}
