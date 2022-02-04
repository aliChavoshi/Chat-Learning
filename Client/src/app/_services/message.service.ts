import { environment } from 'src/environments/environment';
import { MessageParams, IMessage } from './../_models/message';
import { Injectable } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private baseUrl = environment.baseUrl;
  private messageParams: MessageParams = new MessageParams();

  constructor(private http: HttpClient) {}

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
