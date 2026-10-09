import { Component, inject } from '@angular/core';
import { ShopHeader } from './components/shop-header/shop-header';
import { ShopFooter } from './components/shop-footer/shop-footer';
import { ShoppingListService } from './services/shopping-list';
import { ShoppingList } from './components/shopping-list/shopping-list';
import {
  FormBuilder,
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
  private readonly formBuilder = inject(FormBuilder);

  // readonly renameForm = new FormGroup({
  //   listId: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
  //   title: new FormControl('', {
  //     nonNullable: true,
  //     validators: [Validators.required, Validators.minLength(2)],
  //   }),
  // });

  readonly renameForm = this.formBuilder.group({
    listId: ['', Validators.required],
    title: ['', [Validators.required, Validators.minLength(2)]],
  });

  newListTitle = '';

  togglePurchased(event: { listId: string; itemId: number }): void {
    this.listService.togglePurchased(event);
  }

  loadLists(): void {
    this.listService.getLists();
  }

  createList(): void {
    const title = this.newListTitle.trim();
    if (title.length < 2) {
      this.newListTitle = title;
      return;
    }

    this.listService.createList(title);
  }

  renameList(): void {
    if (this.renameForm.invalid) return;

    const listId = this.renameForm.controls.listId.value;
    const title = this.renameForm.controls.title.value?.trim();
    if (title!.length < 2) {
      this.renameForm.controls.title.setValue(title!);
      this.renameForm.controls.title.markAsTouched();
      return;
    }
    if (listId === '') return;

    this.listService.renameList(listId!, title!);
  }

  deleteList(): void {
    const listId = this.renameForm.controls.listId.value;
    if (listId === '') return;

    this.listService.deleteList(listId!);
    this.renameForm.reset();
  }
}
