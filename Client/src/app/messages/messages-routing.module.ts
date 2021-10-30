import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeMessageComponent } from './home-message/home-message.component';

const routes: Routes = [{ path: '', component: HomeMessageComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MessagesRoutingModule {}
