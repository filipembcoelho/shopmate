import { ShoppingListItem } from './ShoppingListItem';

export interface ShoppingListData {
  id: string;
  title: string;
  items: ShoppingListItem[];
}
