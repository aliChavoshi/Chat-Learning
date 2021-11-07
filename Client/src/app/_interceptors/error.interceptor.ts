import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private toast: ToastrService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error) {
          switch (error.status) {
            case 404:
              this.router.navigateByUrl('not-found');
              break;
            case 400:
              //validation Errors
              if (error.error.errors) {
                const modelStateErrors: string[] = [];
                for (const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modelStateErrors.push(error.error.errors[key]);
                  }
                }
                throw modelStateErrors;
              }
              //Bad request 400
              else {
                this.toast.error(
                  error.error.message,
                  'کد خطا : ' + error.status
                );
              }
              break;
            case 500:
              this.toast.error(
                'خطایی در سمت سرور رخ داده است لطفا مجددا تلاش کنید',
                'کد خطا : ' + error.status.toString()
              );
              break;

            default:
              this.toast.error(error.error.message, 'کد خطا :' + error.status);
              break;
          }
        }
        return throwError(error);
      })
    );
  }
}
