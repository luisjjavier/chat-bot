import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { JwtModule } from '@auth0/angular-jwt';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NavMenuComponent } from './componets/nav-menu/nav-menu.component';
import { AccountLayoutComponent } from './layouts/account-layout/account-layout.component';
import { AuthorizeLayoutComponent } from './layouts/authorize-layout/authorize-layout.component';
import { RegisterComponent } from './pages/register/register.component';
import { LoginComponent } from './pages/login/login.component';
import { ChatRoomComponent } from './pages/chat-room/chat-room.component';
import { TokenInterceptor } from './services/token.interceptor';
import { CreateOrJoinChatRoomComponent } from './pages/create-or-join-chat-room/create-or-join-chat-room.component';


function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    AccountLayoutComponent,
    AuthorizeLayoutComponent,
    RegisterComponent,
    LoginComponent,
    ChatRoomComponent,
    CreateOrJoinChatRoomComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter
      }
    })
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
