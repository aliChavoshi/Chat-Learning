import { RolesModalComponent } from './../../_modals/roles-modal/roles-modal.component';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Component, OnInit } from '@angular/core';
import { IUser } from 'src/app/_models/account';
import { AdminService } from '../admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css'],
})
export class UserManagementComponent implements OnInit {
  users: IUser[];
  //modal
  bsModalRef: BsModalRef; //roles component

  constructor(
    private adminService: AdminService,
    private modalService: BsModalService
  ) {}

  ngOnInit(): void {
    this.getUsersWithRoles();
  }
  onEditModal() {
    const init = {
      initialState: {
        list: ['test', 'test1', 'test2'],
        title: 'Ali Chavoshi',
      },
    };
    this.bsModalRef = this.modalService.show(RolesModalComponent, init);
    this.bsModalRef.content.closeBtnName = 'بسته';
  }
  getUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe((res) => {
      this.users = res;
    });
  }
}
