export interface ShoppingListItem {
  id: number;
  name: string;
  quantity: number;
  unit?: number;
  purchased: boolean;
}

export interface ShoppingListItemApiDto {
  id: number;
  name: string;
  quantity: number;
  unit: number;
  isCompleted: boolean;
}
