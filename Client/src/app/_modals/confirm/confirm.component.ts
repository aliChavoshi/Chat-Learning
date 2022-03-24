import { BsModalRef } from 'ngx-bootstrap/modal';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-confirm',
  templateUrl: './confirm.component.html',
  styleUrls: ['./confirm.component.css'],
})
export class ConfirmComponent implements OnInit {
  @Output() result = new EventEmitter<boolean>();
  title = '';
  message = '';
  btnOkText = 'Yes';
  btnCancelText = 'No';

  constructor(public bsModalRef: BsModalRef) {}

  ngOnInit(): void {}
  confirm() {
    this.result.emit(true);
    this.bsModalRef.hide();
  }
  decline() {
    this.result.emit(false);
    this.bsModalRef.hide();
  }
}
