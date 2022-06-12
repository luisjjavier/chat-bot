import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as SignalR from '@microsoft/signalr'
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChatRoomService {
  baseUrl = environment.apiUrl;
  private hubConnection!: SignalR.HubConnection;

  constructor(private readonly http: HttpClient) {
  }

  public startConnection = async () => {
    this.hubConnection = new SignalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}/chat-room/hub`, SignalR.HttpTransportType.ServerSentEvents)
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

  onUserEnrollmentMessage = () => {
    this.hubConnection.on('OnUserEnrollmentMessage', (message) => {
      console.log(message);
    })
  }

  createChatRoom(value: string) {
    return this.http.post(`${this.baseUrl}/chat-room`, {
      name: value
    })
  }
}
