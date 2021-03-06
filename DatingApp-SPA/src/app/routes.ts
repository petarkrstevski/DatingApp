import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberListsComponent } from './member-lists/member-lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  {
    path:'',
    runGuardsAndResolvers:'always',
    canActivate:[AuthGuard],
    children : [
      { path: 'lists', component: ListsComponent },
      { path: 'members', component: MemberListsComponent },
      { path: 'messages', component: MessagesComponent },
    ]
  },
  { path: '**', redirectTo:'', pathMatch: 'full' }

];


