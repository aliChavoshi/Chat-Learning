import { IUser } from 'src/app/_models/account';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { MessageParams, IMessage } from './../_models/message';
import { Injectable } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';
import { PaginatedResult } from '../_models/pagination';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private baseUrl = environment.baseUrl;
  private messageParams: MessageParams = new MessageParams();
  //signalR
  private hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private messageThreadSource = new BehaviorSubject<IMessage[]>([]); //next
  messageThreadSource$ = this.messageThreadSource.asObservable(); //subscribe

  constructor(private http: HttpClient) {}

  createHubConnection(user: IUser, otherUser: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + '/message?user=' + otherUser, {
        accessTokenFactory: () => user?.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((err) => {
      console.log(err);
    });
    this.hubConnection.on('ReceiveMessageThread', (messages: IMessage[]) => {
      this.messageThreadSource.next(messages);
    });
  }
  stopHubConnection() {
    this.hubConnection.stop().catch((err) => {
      console.log(err);
    });
  }
  getMessages(messageParams: MessageParams) {
    //get data from api
    let params = this.setParams(messageParams);
    return this.http.get<PaginatedResult<IMessage[]>>(
      `${this.baseUrl}/message`,
      {
        params,
      }
    );
  }
  //default
  getMessageThread(userName: string) {
    return this.http.get<IMessage[]>(
      `${this.baseUrl}/Message/Thread/${userName}`
    );
  }
  sendMessage(recipientUserName: string, content: string) {
    return this.http.post<IMessage>(`${this.baseUrl}/message`, {
      content,
      recipientUserName,
    });
  }
  private setParams(messageParams: MessageParams) {
    let params = new HttpParams();
    if (messageParams.pageNumber !== null && messageParams.pageSize !== null) {
      params = params.append('pageNumber', messageParams.pageNumber.toString());
      params = params.append('pageSize', messageParams.pageSize.toString());
      params = params.append('container', messageParams.container.toString());
    }
    return params;
  }
  //message Params
  getMessageParams() {
    return this.messageParams;
  }
  setMessageParams(messageParams: MessageParams) {
    this.messageParams = messageParams;
  }
  resetMessageParams() {
    this.messageParams = new MessageParams();
    return this.messageParams;
  }
}
