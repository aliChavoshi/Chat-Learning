import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MembersRoutingModule } from './members-routing.module';
import { ListMemberComponent } from './list-member/list-member.component';
import { HomeMemberComponent } from './home-member/home-member.component';
import { DetailMemberComponent } from './detail-member/detail-member.component';
import { CardMemberComponent } from './card-member/card-member.component';

@NgModule({
  declarations: [
    ListMemberComponent,
    HomeMemberComponent,
    DetailMemberComponent,
    CardMemberComponent,
  ],
  imports: [CommonModule, MembersRoutingModule],
})
export class MembersModule {}
