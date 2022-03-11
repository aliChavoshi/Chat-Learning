import { BehaviorSubject } from 'rxjs';
import { IUser } from './../_models/account';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  //base
  private hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private usersOnline = new BehaviorSubject<string[]>([]); //next
  public usersOnline$ = this.usersOnline.asObservable(); //sub

  constructor(private toast: ToastrService) {}

  createHubConnection(user: IUser) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + '/presence', {
        accessTokenFactory: () => user?.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((err) => {
      this.toast.error('Hub connection error');
    });

    this.hubConnection.on('UserIsOnline', (userName: string) => {
      this.toast.success(userName + ' is online');
    });
    this.hubConnection.on('UserIsOffline', (userName: string) => {
      this.toast.error(userName + ' is offline');
    });
    this.hubConnection.on('GetOnlineUsers', (users: string[]) => {
      this.usersOnline.next(users);
    });
  }

  stopHubConnection() {
    this.hubConnection.stop().catch((err) => {
      this.toast.error('stop Hub connection error');
    });
  }
}