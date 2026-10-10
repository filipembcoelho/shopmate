export interface ShoppingListItem {
  id: number;
  name: string;
  quantity: number;
  unit?: number;
  purchased: boolean;
}
