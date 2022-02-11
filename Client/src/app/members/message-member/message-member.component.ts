import { Component, Input, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { IMessage } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-message-member',
  templateUrl: './message-member.component.html',
  styleUrls: ['./message-member.component.css']
})
export class MessageMemberComponent implements OnInit {
  private sub = new Subscription();
  @Input() userName;
  messages: IMessage[] = [];

  constructor(private messageService: MessageService) {}

  ngOnInit(): void {}
  loadMessageThread() {
    const sub$ = this.messageService
      .getMessageThread(this.userName)
      .subscribe((res) => {
        this.messages = res;
      });
    this.sub.add(sub$);
  }
  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }
}
