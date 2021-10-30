import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeMemberComponent } from './home-member/home-member.component';
import { ListMemberComponent } from './list-member/list-member.component';

//localhost:4200/members
const routes: Routes = [
  {
    path: '',
    component: HomeMemberComponent,
    children: [{ path: '', component: ListMemberComponent }],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MembersRoutingModule {}
