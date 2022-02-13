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
  onEditModal(user: IUser) {
    const init: any = {
      class: 'modal-dialog-centered',
      initialState: {
        title: 'Edit Roles',
        user,
        roles: this.getRolesArray(user), //name,checked
      },
    };
    this.bsModalRef = this.modalService.show(RolesModalComponent, init);
    // this.bsModalRef.content.closeBtnName = 'بسته';
    this.bsModalRef.content?.updateRoles.subscribe((roles) => {
      const rolesToUpdate = {
        rolesName: [
          ...roles.filter((r) => r.checked === true).map((el) => el.name),
        ],
      };
      if (rolesToUpdate && rolesToUpdate.rolesName.length > 0) {
        this.adminService
          .editUserRoles(user.userName, rolesToUpdate.rolesName)
          .subscribe((response: any) => {
            user.roles = [...response];
            console.log(response);
          });
      }
    });
  }
  getUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe((res) => {
      this.users = res;
    });
  }

  private getRolesArray(user: IUser) {
    const roles = []; // [] //return
    const userRoles = user.roles; //string[]
    const availableRoles: { name: string; value: string; checked: boolean }[] =
      [
        { name: 'admin', value: 'admin', checked: false },
        { name: 'member', value: 'member', checked: false },
        { name: 'superAdmin', value: 'superAdmin', checked: false },
      ]; //database

    for (let i = 0; i < availableRoles.length; i++) {
      let isMatch = false;
      for (let j = 0; j < userRoles.length; j++) {
        if (availableRoles[i].name === userRoles[j]) {
          isMatch = true;
          availableRoles[i].checked = true;
          roles.push(availableRoles[i]);
          break;
        }
      }
      if (!isMatch) {
        availableRoles[i].checked = false;
        roles.push(availableRoles[i]);
      }
    }
    return roles;
  }
}
