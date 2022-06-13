import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as SignalR from '@microsoft/signalr'
import { Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { ClientMessage } from '../models/client-message';

@Injectable({
  providedIn: 'root'
})
export class ChatRoomService {
  baseUrl = environment.apiUrl;
  private hubConnection!: SignalR.HubConnection;
  newMessage = new Subject<ClientMessage>();
  newUserAdded = new Subject<ClientMessage>();
  constructor(private readonly http: HttpClient) {
  }

  public startConnection = async () => {
    this.hubConnection = new SignalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}/chat-room/hub`)
      .withAutomaticReconnect()
      .build();

    this.hubConnection.serverTimeoutInMilliseconds = 36000000;
    this.hubConnection.keepAliveIntervalInMilliseconds = 1800000;

    await this.hubConnection
      .start()
      .catch(console.error);
  };

  public async stopConnection() {
    await this.hubConnection.stop();
  }

  onUserEnrollmentMessage = async (roomCode: string, username: string) => {
    await this.hubConnection.invoke('EnrollUserToChatRoom', roomCode, username)
    this.hubConnection.on('OnUserEnrollmentMessage', (message: ClientMessage) => {
      this.newUserAdded.next(message)
    })
  }

  onChatRoomMessageReceived (){
    this.hubConnection.on('OnChatRoomMessage', (message:ClientMessage) =>{
      this.newMessage.next(message);
    })
  }
  createChatRoom(value: string) {
    return this.http.post(`${this.baseUrl}/chat-room`, {
      name: value
    })
  }

  sendNewMessage(message: ClientMessage) {
    return this.hubConnection.invoke("SendMessage", message);
  }

  getAllMessages(roomCode: string) {
    return this.http.get<ClientMessage[]>(`${this.baseUrl}/chat-room/${roomCode}/messages`)
  }
}
