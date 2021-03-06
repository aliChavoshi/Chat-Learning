import { Router } from '@angular/router';
import { take } from 'rxjs/operators';
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

  constructor(private toast: ToastrService, private router: Router) {}

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
    //notification get new message
    this.hubConnection.on('NewMessageReceived', ({ userName, content }) => {
      this.toast
        .info(userName + ' ' + 'has send you a message')
        .onTap.pipe(take(1))
        .subscribe(() => {
          this.router.navigateByUrl('/members/' + userName + '?tab=2');
        });
    });
  }

  stopHubConnection() {
    this.hubConnection.stop().catch((err) => {
      this.toast.error('stop Hub connection error');
    });
  }
}
