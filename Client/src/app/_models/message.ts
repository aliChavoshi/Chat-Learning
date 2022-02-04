export interface IMessage {
  id: number;
  senderId: number;
  senderUserName: string;
  senderPhotoUrl: string;
  receiverId: number;
  receiverUserName: string;
  receiverPhotoUrl: string;
  content: string;
  dateRead: Date;
  messageSent: string;
  isRead: boolean;
}
export class MessageParams {
  container: string = 'Inbox';
  pageNumber: number = 1;
  pageSize: number = 10;
}
