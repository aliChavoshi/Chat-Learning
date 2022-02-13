import { ToastrService } from 'ngx-toastr';
import { IUser } from './../../_models/account';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css'],
})
export class RolesModalComponent implements OnInit {
  @Output() updateRoles = new EventEmitter<any>();
  title = 'my Title';
  closeBtnName = 'close';
  user: IUser;
  roles: any[];

  constructor(public bsModalRef: BsModalRef, private toast: ToastrService) {}

  ngOnInit(): void {}

  changeUpdatedRoles() {
    this.updateRoles.emit(this.roles);
    this.bsModalRef.hide();
    this.toast.success('roles updated');
  }
}
