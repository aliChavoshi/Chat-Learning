import { Component, Input, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { IMessage } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-message-member',
  templateUrl: './message-member.component.html',
  styleUrls: ['./message-member.component.css'],
})
export class MessageMemberComponent implements OnInit {
  private sub = new Subscription();
  messageContent = '';
  loading = false;
  @Input() messages: IMessage[] = [];
  @Input() userName : string;

  constructor() {}

  ngOnInit(): void {}
  onSubmit() {}
  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }
}
