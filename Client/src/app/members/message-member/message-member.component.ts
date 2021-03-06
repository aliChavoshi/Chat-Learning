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
  @Input() userName: string;

  constructor(public messageService: MessageService) {}

  ngOnInit(): void {}
  onSubmit() {
    this.messageService
      .addMessage(this.messageContent, this.userName)
      .then((response) => {
        this.messageContent = '';
      });
  }
  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }
}
