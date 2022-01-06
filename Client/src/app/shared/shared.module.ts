import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

//ngx-bootstrap
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
//ngx-toast
import { ToastrModule } from 'ngx-toastr';
//tabs
import { TabsModule } from 'ngx-bootstrap/tabs';
//gallery
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
//spinner loading
import { NgxSpinnerModule } from 'ngx-spinner';
//file uploader
import { FileUploadModule } from 'ng2-file-upload';
//pipes
import { ShowEnumPipe } from './_pipes/show-enum.pipe';
//timeage
import { TimeagoModule } from 'ngx-timeago';
@NgModule({
  declarations: [ShowEnumPipe],
  imports: [
    CommonModule,
    //ngx-bootstrap
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
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
    NgxSpinnerModule,
    //file uploader
    FileUploadModule,
    //timeage
    TimeagoModule.forRoot(),
  ],
  exports: [
    //ngx-bootstrap
    BsDropdownModule,
    BsDatepickerModule,
    PaginationModule,
    //ngx-toastr
    ToastrModule,
    TabsModule,
    NgxGalleryModule,
    //spinner loading
    NgxSpinnerModule,
    //file uploader
    FileUploadModule,
    //pipes
    ShowEnumPipe,
    TimeagoModule,
  ],
})
export class SharedModule {}
