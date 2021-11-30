import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

//ngx-bootstrap
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
//ngx-toast
import { ToastrModule } from 'ngx-toastr';
//tabs
import { TabsModule } from 'ngx-bootstrap/tabs';
//gallery
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
//spinner loading
import { NgxSpinnerModule } from "ngx-spinner";
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
    //tabs
    TabsModule.forRoot(),
    NgxGalleryModule,
    //spinner loading
    NgxSpinnerModule
  ],
  exports: [
    //ngx-bootstrap
    BsDropdownModule,
    //ngx-toastr
    ToastrModule,
    TabsModule,
    NgxGalleryModule,
    //spinner loading
    NgxSpinnerModule
  ],
})
export class SharedModule {}
