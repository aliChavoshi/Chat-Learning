import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnInit,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { IMessage } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-message-member',
  templateUrl: './message-member.component.html',
  styleUrls: ['./message-member.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MessageMemberComponent implements OnInit {
  private sub = new Subscription();
  messageContent = '';
  loading = false;
  @Input() messages: IMessage[] = [];
  @Input() userName: string;

  constructor(private messageService: MessageService) {}

  ngOnInit(): void {}
  onSubmit() {
    const sub$ = this.messageService
      .sendMessage(this.userName, this.messageContent)
      .subscribe((message) => {
        this.messages.push(message);
        this.messageContent = '';
      });
    this.sub.add(sub$);
  }
  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }
}
