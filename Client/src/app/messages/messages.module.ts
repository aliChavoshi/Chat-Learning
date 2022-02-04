import { FormsModule } from '@angular/forms';
import { SharedModule } from './../shared/shared.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MessagesRoutingModule } from './messages-routing.module';
import { HomeMessageComponent } from './home-message/home-message.component';

@NgModule({
  declarations: [HomeMessageComponent],
  imports: [CommonModule, MessagesRoutingModule, SharedModule, FormsModule],
})
export class MessagesModule {}
