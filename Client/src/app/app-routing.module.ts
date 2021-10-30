import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'members',
    loadChildren: () =>
      import('./members/members.module').then((x) => x.MembersModule),
  },
  {
    path: 'lists',
    loadChildren: () => import('./list/list.module').then((x) => x.ListModule),
  },
  {
    path: 'messges',
    loadChildren: () =>
      import('./messages/messages.module').then((x) => x.MessagesModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
