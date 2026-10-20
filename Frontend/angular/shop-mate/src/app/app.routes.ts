import { Routes } from '@angular/router';
import { ShoppingLists } from './pages/shopping-lists/shopping-lists';
import { ListEditor } from './pages/list-editor/list-editor';
import { NotFound } from './pages/not-found/not-found';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: 'login', component: Login, title: 'ShopMate - Sign in' },
  { path: 'register', component: Register, title: 'ShopMate - Create account' },
  { path: '', component: ShoppingLists, canActivate: [authGuard], title: 'ShopMate - Lists' },
  {
    path: 'lists/new',
    component: ListEditor,
    canActivate: [authGuard],
    title: 'ShopMate - New List',
  },
  {
    path: 'lists/:id/edit',
    component: ListEditor,
    canActivate: [authGuard],
    title: 'ShopMate - Edit List',
  },
  { path: 'not-found', component: NotFound, title: 'ShopMate - Page Not Found' },
  { path: '**', component: NotFound, title: 'ShopMate - Page Not Found' },
];
