import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { HubConnection } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  //base
  private hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;

  constructor(private toast: ToastrService) {}

  createHubConnection() {}
}
