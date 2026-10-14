import { ShoppingListItem, ShoppingListItemApiDto } from './ShoppingListItem';

export interface ShoppingListData {
  id: string;
  title: string;
  items: ShoppingListItem[];
}

export interface ShoppingListApiDto {
  id: number;
  name: string;
  items: ShoppingListItemApiDto[];
}
