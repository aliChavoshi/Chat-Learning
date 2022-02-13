import { IUser } from './../_models/account';
import { AdminService } from './admin.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css'],
})
export class AdminComponent implements OnInit {
  title: string;

  constructor(private route: ActivatedRoute) {
    this.title = this.route?.snapshot?.data?.title ?? '';
  }

  ngOnInit(): void {}
}
