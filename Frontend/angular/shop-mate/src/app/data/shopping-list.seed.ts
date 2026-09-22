import { ShoppingListData } from '../Model/ShoppingListData';

export function createSeedShoppingLists(): ShoppingListData[] {
  return [
    {
      id: 1,
      title: 'Weekly groceries',
      items: [
        { id: 1, name: 'arroz doce', quantity: 3, purchased: false },
        { id: 2, name: 'leite', quantity: 2, purchased: false },
      ],
    },
    {
      id: 2,
      title: 'Weekend plans',
      items: [
        { id: 1, name: 'Massa', quantity: 2, purchased: false },
        { id: 2, name: 'Pão', quantity: 1, purchased: false },
      ],
    },
  ];
}
