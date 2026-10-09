import { Routes } from '@angular/router';
import { ShoppingLists } from './pages/shopping-lists/shopping-lists';
import { ListEditor } from './pages/list-editor/list-editor';

export const routes: Routes = [
  { path: '', component: ShoppingLists, title: 'ShopMate - Lists' },
  { path: 'lists/new', component: ListEditor, title: 'ShopMate - New List' },
  { path: 'lists/:id/edit', component: ListEditor, title: 'ShopMate - Edit List' },
  { path: '**', redirectTo: '', pathMatch: 'full' },
];

/*

localhost:4200/lists/2/edit



*/
