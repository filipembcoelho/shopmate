import { ShoppingListItem } from './ShoppingListItem';

export interface ShoppingListData {
  id: number;
  title: string;
  items: ShoppingListItem[];
}
