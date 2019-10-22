import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberListsComponent } from './member-lists/member-lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'lists', component: ListsComponent },
  { path: 'members', component: MemberListsComponent, canActivate:[AuthGuard] },
  { path: 'messages', component: MessagesComponent },
  { path: '**', redirectTo:'home', pathMatch: 'full' }
];


