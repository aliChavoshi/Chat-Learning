import { IMember } from 'src/app/_models/member';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-photo-edit',
  templateUrl: './photo-edit.component.html',
  styleUrls: ['./photo-edit.component.css'],
})
export class PhotoEditComponent implements OnInit {
  @Input() member: IMember;
  constructor() {}

  ngOnInit(): void {}
}
