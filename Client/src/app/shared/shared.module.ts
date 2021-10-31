import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

//ngx-bootstrap
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
//ngx-toast
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    //ngx-bootstrap
    BsDropdownModule.forRoot(),
    //ngx-toastr
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      timeOut: 5000,
      progressBar: true,
      progressAnimation: 'increasing',
    }),
  ],
  exports: [
    //ngx-bootstrap
    BsDropdownModule,
    //ngx-toastr
    ToastrModule,
  ],
})
export class SharedModule {}
