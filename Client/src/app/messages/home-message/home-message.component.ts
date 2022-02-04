import { PaginatedResult } from 'src/app/_models/pagination';
import { IMessage, MessageParams } from './../../_models/message';
import { MessageService } from './../../_services/message.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home-message',
  templateUrl: './home-message.component.html',
  styleUrls: ['./home-message.component.css'],
})
export class HomeMessageComponent implements OnInit {
  messageParams: MessageParams;
  result: PaginatedResult<IMessage[]>;

  constructor(private messageService: MessageService) {
    this.messageParams = this.messageService.getMessageParams();
  }

  ngOnInit(): void {
    this.loadMessages();
  }
  loadMessages() {
    this.messageService
      .getMessages(this.messageParams)
      .subscribe((response) => {
        this.result = response;
      });
  }
  pageChanged(event: any): void {
    this.messageParams.pageNumber = event.page;
    this.messageService.setMessageParams(this.messageParams);
    this.loadMessages();
  }
}
