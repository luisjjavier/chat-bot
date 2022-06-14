import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ClientMessage } from '../../models/client-message';
import { AuthService } from '../../services/auth.service';
import { ChatRoomService } from '../../services/chat-room.service';

@Component({
  selector: 'app-chat-room',
  templateUrl: './chat-room.component.html',
  styleUrls: ['./chat-room.component.scss']
})
export class ChatRoomComponent implements OnInit, OnDestroy {
  currentUserName: string ='';
  @ViewChild('chat', { static: false }) private chatElement!: ElementRef  ;

  subscription: Subscription[] =[];

  message = new FormControl('');

  currentMessages: ClientMessage[] = [];
  chatRoomCode: string = '';

  constructor(
    private authService: AuthService, private router: Router,
    private readonly chatRoomService: ChatRoomService,
    private readonly activeRoute: ActivatedRoute
  ) { }

  async ngOnInit() {
    this.currentUserName = this.authService.getCurrentUser().userName;
    console.log(this.currentUserName);
    ({roomCode: this.chatRoomCode} = this.activeRoute.snapshot.params);
    await this.chatRoomService.startConnection();
    await this.chatRoomService.onUserEnrollmentMessage(this.chatRoomCode, this.currentUserName);
    this.chatRoomService.onChatRoomMessageReceived();

    this.subscription.push( this.chatRoomService.newUserAdded.subscribe({
      next: message => this.currentMessages.push(message)
    }))

    this.subscription.push( this.chatRoomService.newMessage.subscribe({
      next: message => {
        this.currentMessages.push(message)
        this.chatScrollToBottom();
      }
    }))

    this.chatRoomService.getAllMessages(this.chatRoomCode).subscribe({
      next: messages => {
        this.currentMessages.push(...messages)
        this.chatScrollToBottom();
      }
    })
  }

  private chatScrollToBottom() {
    setTimeout(() => {
      this.chatElement.nativeElement.scrollTop = this.chatElement.nativeElement.scrollHeight;
    }, 100);
  }

  async checkIsEnterKey(event: any) {
    const enterKeyCode = 13;
    if (event.keyCode === enterKeyCode) {
      await this.send();
    }
  }

  async send() {
    const newMessage: ClientMessage = {
      clientUserName: this.currentUserName,
      sentOnUtc: new Date(),
      message: this.message.value,
      roomCode: this.chatRoomCode
    };

   await this.chatRoomService.sendNewMessage(newMessage);

   this.message.reset();
  }

  getMessageStyleClassByUserName(userName: string) {
    if (userName === "#Bot") {
      return "bot";
    } else if (userName === this.currentUserName) {
      return "me";
    }else if (userName === "#system"){
      return "system"
    }
      else {
      return "you";
    }
  }

  ngOnDestroy(): void {
    this.subscription.forEach(value => value.unsubscribe());
  }
}
