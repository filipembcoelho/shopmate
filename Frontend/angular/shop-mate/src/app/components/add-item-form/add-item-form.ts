import { Component, inject, input } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ShoppingListService } from '../../services/shopping-list';

@Component({
  selector: 'app-add-item-form',
  imports: [ReactiveFormsModule],
  templateUrl: './add-item-form.html',
})
export class AddItemForm {
  readonly listId = input.required<string>();
  readonly listService = inject(ShoppingListService);
  private readonly formBuilder = inject(FormBuilder);

  readonly itemForm = this.formBuilder.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    quantity: [1, [Validators.required, Validators.min(1)]],
    unit: ['1', Validators.required],
  });

  addItem(): void {
    if (this.itemForm.invalid || this.listService.isAddingItem()) return;

    const values = this.itemForm.getRawValue();
    const name = values.name.trim();
    if (name.length < 2) {
      this.itemForm.controls.name.setValue(name);
      this.itemForm.controls.name.markAsTouched();
      return;
    }

    this.listService.addItem(
      this.listId(),
      name,
      Number(values.quantity),
      Number(values.unit),
      () => this.itemForm.reset({ name: '', quantity: 1, unit: '1' }),
    );
  }
}
