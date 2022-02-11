import { PaginatedResult } from 'src/app/_models/pagination';
import { IMessage, MessageParams } from './../../_models/message';
import { MessageService } from './../../_services/message.service';
import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-home-message',
  templateUrl: './home-message.component.html',
  styleUrls: ['./home-message.component.css'],
})
export class HomeMessageComponent implements OnInit {
  messageParams: MessageParams;
  result: PaginatedResult<IMessage[]>;
  loading = false;

  constructor(private messageService: MessageService) {
    this.messageParams = this.messageService.getMessageParams();
  }

  ngOnInit(): void {
    this.loadMessages();
  }
  loadMessages() {
    this.loading = true;
    this.messageService
      .getMessages(this.messageParams)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((response) => {
        this.result = response;
      });
  }
  changeContainer(container: string) {
    this.messageParams.container = container;
    this.messageService.setMessageParams(this.messageParams);
    this.loadMessages();
  }
  pageChanged(event: any): void {
    this.messageParams.pageNumber = event.page;
    this.messageService.setMessageParams(this.messageParams);
    this.loadMessages();
  }
}
