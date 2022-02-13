import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css'],
})
export class RolesModalComponent implements OnInit {
  title = 'my Title';
  closeBtnName = 'close';
  list: any[] = [];

  constructor(public bsModalRef: BsModalRef) {}

  ngOnInit(): void {}
}
