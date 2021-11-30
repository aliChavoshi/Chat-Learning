import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpEventType,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { BusyService } from '../_services/Busy.service';
import { tap } from 'rxjs/operators';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  constructor(private busyService: BusyService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    this.busyService.showBusy();
    return next.handle(request).pipe(
      tap((event) => {
        //send request
        if (event.type === HttpEventType.Sent) {
        }
        //get response
        if (event.type === HttpEventType.Response) {
          this.busyService.hideBusy();
        }
      })
    );
  }
}
