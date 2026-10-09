import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ShoppingListService } from '../../services/shopping-list';
import { ShoppingList } from '../../components/shopping-list/shopping-list';

@Component({
  imports: [ShoppingList, FormsModule, ReactiveFormsModule],
  selector: 'app-shopping-lists',
  styleUrl: './shopping-lists.css',
  templateUrl: './shopping-lists.html',
})
export class ShoppingLists implements OnInit, OnDestroy {
  readonly listService = inject(ShoppingListService);
  private readonly formBuilder = inject(FormBuilder);

  constructor() {}

  ngOnInit() {
    this.loadLists();
    console.log('ShoppingLists component initialized');
  }

  ngOnDestroy() {
    // cleanup logic if needed
    console.log('ShoppingLists component destroyed');
  }

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
