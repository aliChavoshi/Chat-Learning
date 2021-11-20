import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DetailMemberComponent } from './detail-member/detail-member.component';
import { HomeMemberComponent } from './home-member/home-member.component';
import { ListMemberComponent } from './list-member/list-member.component';

//localhost:4200/members
const routes: Routes = [
  {
    path: '',
    component: HomeMemberComponent,
    children: [
      { path: '', component: ListMemberComponent },
      //localhost:4200/members/lisa
      { path: ':username', component: DetailMemberComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MembersRoutingModule {}
