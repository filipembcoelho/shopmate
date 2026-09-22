import { describe, expect, it } from 'vitest';
import { createSeedShoppingLists } from './shopping-list.seed';

describe('createSeedShoppingLists', () => {
  it('returns two distinct titled lists with items', () => {
    const lists = createSeedShoppingLists();

    expect(lists).toHaveLength(2);
    expect(new Set(lists.map((list) => list.id)).size).toBe(2);
    expect(lists.every((list) => list.title.trim() && list.items.length > 0)).toBe(true);
  });

  it('returns fresh lists and items each time', () => {
    const first = createSeedShoppingLists();
    const second = createSeedShoppingLists();

    first[0].items[0].purchased = true;

    expect(second[0].items[0].purchased).toBe(false);
  });
});
