import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { ClientMessage } from '../../models/client-message';
import { AuthService } from '../../services/auth.service';
import { ChatRoomService } from '../../services/chat-room.service';

@Component({
  selector: 'app-chat-room',
  templateUrl: './chat-room.component.html',
  styleUrls: ['./chat-room.component.scss']
})
export class ChatRoomComponent implements OnInit {
  currentUserName: string ='';
  @ViewChild('chat', { static: false }) private chatElement!: ElementRef  ;
  message = new FormControl('');

  currentMessages: ClientMessage[] = [];

  constructor(
    private authService: AuthService, private router: Router,
    private readonly chatRoomService: ChatRoomService
  ) { }

  async ngOnInit() {
    this.currentUserName = this.authService.getCurrentUser().userName;
    await this.chatRoomService.startConnection();
  }

  private chatScrollToBottom() {
    setTimeout(() => {
      this.chatElement.nativeElement.scrollTop = this.chatElement.nativeElement.scrollHeight;
    }, 100);
  }

  checkIsEnterKey(event: any) {
    const enterKeyCode = 13;
    if (event.keyCode === enterKeyCode) {
      this.send();
    }
  }

  send() {

  }

  getMessageStyleClassByUserName(userName: string) {
    if (userName === "#BOT") {
      return "bot";
    } else if (userName === this.currentUserName) {
      return "me";
    } else {
      return "you";
    }
  }
}
