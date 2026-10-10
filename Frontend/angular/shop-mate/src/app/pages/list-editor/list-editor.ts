import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ShoppingListService } from '../../services/shopping-list';

@Component({
  selector: 'app-list-editor',
  imports: [ReactiveFormsModule, RouterLink],
  styleUrl: './list-editor.css',
  templateUrl: './list-editor.html',
})
export class ListEditor implements OnInit {
  readonly listService = inject(ShoppingListService);
  private readonly formBuilder = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  listId: string | null = null;
  isLoading = false;
  loadFailed = false;

  readonly listForm = this.formBuilder.nonNullable.group({
    title: ['', [Validators.required, Validators.minLength(2)]],
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id === null) return;

    this.listId = id;
    this.isLoading = true;
    this.listService.getList(id).subscribe({
      next: (list) => {
        this.listForm.controls.title.setValue(list.title);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Could not load shopping list', error);
        this.isLoading = false;
        this.loadFailed = true;
      },
    });
  }

  saveList(): void {
    if (this.listForm.invalid || this.isLoading || this.loadFailed) return;

    const title = this.listForm.controls.title.value.trim();
    if (title.length < 2) {
      this.listForm.controls.title.setValue(title);
      this.listForm.controls.title.markAsTouched();
      return;
    }

    if (this.listId === null) {
      this.listService.createList(title, () => this.router.navigateByUrl('/'));
    } else {
      this.listService.renameList(this.listId, title, () => this.router.navigateByUrl('/'));
    }
  }
}
