import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpHeaders,
  HttpEventType,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/account';
import { take, tap } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  currentUser: User;
  constructor(private accountService: AccountService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    this.accountService.currentUser$.pipe(take(1)).subscribe((user) => {
      this.currentUser = user;
    });
    if (this.currentUser) {
      request = request.clone({
        headers: new HttpHeaders().set(
          'Authorization',
          'Bearer ' + this.currentUser.token
        ),
      });
    }
    return next.handle(request).pipe(
      tap((event) => {
        if (event.type === HttpEventType.Sent) {
        }
        if (event.type === HttpEventType.Response) {
          const token = event?.body?.token;
          if (token) {
            localStorage.setItem('user', JSON.stringify(event.body));
          }
        }
      })
    );
  }
}
