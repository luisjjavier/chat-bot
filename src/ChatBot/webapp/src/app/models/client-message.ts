export interface ClientMessage {
  clientUserName: string;
  sentOnUtc: Date;
  message: string;
  roomCode: string
}
