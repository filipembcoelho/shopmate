import { Component, inject } from '@angular/core';
import { ShopHeader } from './components/shop-header/shop-header';
import { ShopFooter } from './components/shop-footer/shop-footer';
import { ShoppingListService } from './services/shopping-list';
import { ShoppingList } from './components/shopping-list/shopping-list';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  imports: [ShopHeader, ShopFooter, ShoppingList, FormsModule, ReactiveFormsModule],
})
export class App {
  readonly listService = inject(ShoppingListService);

  readonly renameForm = new FormGroup({
    listId: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    title: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(2)],
    }),
  });

  newListTitle = '';

  togglePurchased(event: { listId: number; itemId: number }): void {
    this.listService.togglePurchased(event);
  }

  runSessionDemo() {
    this.listService.getLists();
  }

  createList() {
    console.log('Creating a new list...', this.newListTitle);

    if (this.newListTitle.trim() === '') {
      console.log('List title is empty. Aborting creation.');
      return;
    }

    this.listService.createList(this.newListTitle);
    // this.newListTitle = '';
  }

  renameList() {
    console.log('Renaming list...', this.renameForm.value);
    if (this.renameForm.invalid) {
      return;
    }

    const newTitle = this.renameForm.get('title')?.value;

    if (newTitle?.trim() === '') {
      return;
    }

    console.log('Renaming list to:', newTitle);
    this.listService.renameList(Number(this.renameForm.get('listId')?.value), newTitle!);
  }

  deleteList() {
    const listId = Number(this.renameForm.get('listId')?.value);
    if (isNaN(listId)) {
      return;
    }

    this.listService.deleteList(listId);
  }
}
