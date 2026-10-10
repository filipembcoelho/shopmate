import { Routes } from '@angular/router';
import { ShoppingLists } from './pages/shopping-lists/shopping-lists';
import { ListEditor } from './pages/list-editor/list-editor';
import { NotFound } from './pages/not-found/not-found';

export const routes: Routes = [
  { path: '', component: ShoppingLists, title: 'ShopMate - Lists' },
  { path: 'lists/new', component: ListEditor, title: 'ShopMate - New List' },
  { path: 'lists/:id/edit', component: ListEditor, title: 'ShopMate - Edit List' },
  { path: 'not-found', component: NotFound, title: 'ShopMate - Page Not Found' },
  { path: '**', component: NotFound, title: 'ShopMate - Page Not Found' },
];
