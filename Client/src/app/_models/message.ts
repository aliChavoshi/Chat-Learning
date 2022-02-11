export interface IMessage {
  id: number;
  //send
  senderId: number;
  senderUserName: string;
  senderPhotoUrl: string;
  //receiver
  receiverId: number;
  receiverUserName: string;
  receiverPhotoUrl: string;
  //
  content: string;
  dateRead: Date;
  messageSent: Date;
  isRead: boolean;
}
export class MessageParams {
  container: string = 'Inbox'; //Outbox , Unread , Inbox
  pageNumber: number = 1;
  pageSize: number = 10;
}
