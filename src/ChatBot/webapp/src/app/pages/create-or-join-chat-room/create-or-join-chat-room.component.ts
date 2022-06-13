import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { ChatRoomService } from '../../services/chat-room.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-create-or-join-chat-room',
  templateUrl: './create-or-join-chat-room.component.html',
  styleUrls: ['./create-or-join-chat-room.component.scss']
})
export class CreateOrJoinChatRoomComponent implements OnInit {
  newChatRoomName = new FormControl('');
  joinChatRoom = new FormControl('');
  constructor(private readonly chatRoomService: ChatRoomService,
              private readonly notificationService: NotificationService,
              private readonly router: Router) { }

  ngOnInit(): void {
  }
  onCreateNewRoom (){
    if (this.newChatRoomName.value.trim() === "") {
      return;
    }
    this.notificationService.showLoading();
    this.chatRoomService.createChatRoom(this.newChatRoomName.value).subscribe({
      next: async (value: any) => {
        await this.notificationService.showSuccessMessage(`Your chat room code is ${value.code}`);
        await this.router.navigate(['chatroom', value.code]);
      },
      error: async err => {
        await this.notificationService.showErrorMessage(err.error.error);
      }
    })
  }
  async onJoinToRoom(){
    if (this.joinChatRoom.value.trim() === "") {
      return;
    }
    await this.router.navigate(['chatroom', this.joinChatRoom.value]);
  }
}
