import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  constructor(private spinner: NgxSpinnerService) {}
  private busyRequestCount = new BehaviorSubject<number>(0);
  //1 ->send
  //1-1 = 0 -> response

  showBusy() {
    this.busyRequestCount.next(this.busyRequestCount.value + 1);
    this.spinner.show(undefined, {
      bdColor: 'rgba(0, 0, 0, 0.8)',
      size: 'medium',
      color: '#fff',
      type: 'square-jelly-box',
    });
  }
  hideBusy() {}
}
