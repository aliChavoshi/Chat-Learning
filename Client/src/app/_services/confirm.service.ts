import { Observable } from 'rxjs';
import { ConfirmComponent } from './../_modals/confirm/confirm.component';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ConfirmService {
  bsModalRef: BsModalRef;

  constructor(private modalService: BsModalService) {}

  confirm(
    title = 'Confirmation',
    message = 'Are you sure ypi want to do this ?',
    btnOkText = 'Yes',
    btnCancelText = 'No'
  ): boolean {
    // initialize the modal
    const config = {
      title: title,
      class: 'modal-dialog-centered',
      initialState: {
        title,
        message,
        btnOkText,
        btnCancelText,
      } as any,
    };
    this.bsModalRef = this.modalService.show(ConfirmComponent, config);
    return this.bsModalRef.content.result as boolean;
  }
}
