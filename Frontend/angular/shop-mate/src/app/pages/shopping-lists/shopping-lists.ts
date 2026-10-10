import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ShoppingListService } from '../../services/shopping-list';
import { ShoppingList } from '../../components/shopping-list/shopping-list';

@Component({
  imports: [ShoppingList, RouterLink],
  selector: 'app-shopping-lists',
  styleUrl: './shopping-lists.css',
  templateUrl: './shopping-lists.html',
})
export class ShoppingLists implements OnInit {
  readonly listService = inject(ShoppingListService);

  ngOnInit(): void {
    this.listService.getLists();
  }

  togglePurchased(event: { listId: string; itemId: number }): void {
    this.listService.togglePurchased(event);
  }

  deleteList(listId: string): void {
    this.listService.deleteList(listId);
  }
}
