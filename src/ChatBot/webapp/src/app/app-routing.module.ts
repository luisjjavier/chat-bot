import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountLayoutComponent } from './layouts/account-layout/account-layout.component';
import { AuthorizeLayoutComponent } from './layouts/authorize-layout/authorize-layout.component';
import { ChatRoomComponent } from './pages/chat-room/chat-room.component';
import { CreateOrJoinChatRoomComponent } from './pages/create-or-join-chat-room/create-or-join-chat-room.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { AuthGuard } from './services/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: AccountLayoutComponent,
    children: [
      {
        path: '',
        component: LoginComponent
      },
      {
        path: 'register',
        component: RegisterComponent
      }
    ],

  },
  {
    path: '',
    component: AuthorizeLayoutComponent,
    children: [
      {
        path: 'chatroom/:roomCode',
        canActivate: [AuthGuard],
        component: ChatRoomComponent
      },
      {
        path: 'chatroom',
        canActivate: [AuthGuard],
        component: CreateOrJoinChatRoomComponent
      }
    ]
  },
  {
    path: '**',
    component: LoginComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
