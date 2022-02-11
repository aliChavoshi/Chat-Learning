import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MembersRoutingModule } from './members-routing.module';
import { ListMemberComponent } from './list-member/list-member.component';
import { HomeMemberComponent } from './home-member/home-member.component';
import { DetailMemberComponent } from './detail-member/detail-member.component';
import { SharedModule } from '../shared/shared.module';
import { EditMemberComponent } from './edit-member/edit-member.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PhotoEditComponent } from './photo-edit/photo-edit.component';
import { MessageMemberComponent } from './message-member/message-member.component';

@NgModule({
  declarations: [
    ListMemberComponent,
    HomeMemberComponent,
    DetailMemberComponent,
    EditMemberComponent,
    PhotoEditComponent,
    MessageMemberComponent,
  ],
  imports: [
    CommonModule,
    MembersRoutingModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class MembersModule {}
