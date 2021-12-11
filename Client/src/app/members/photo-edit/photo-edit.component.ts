import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/_services/account.service';
import { User } from 'src/app/_models/account';
import { environment } from 'src/environments/environment';
import { IMember, Photo } from 'src/app/_models/member';
import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-photo-edit',
  templateUrl: './photo-edit.component.html',
  styleUrls: ['./photo-edit.component.css'],
})
export class PhotoEditComponent implements OnInit {
  private backendUrl = environment.baseUrl;
  user: User;
  @Input() member: IMember;

  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  hasAnotherDropZoneOver: boolean;

  constructor(
    private accountService: AccountService,
    private memberService: MemberService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.user = user));
    this.initializeUploader();
  }

  //#region For upload
  initializeUploader() {
    this.uploader = new FileUploader({
      url: `${this.backendUrl}/users/add-photo`,
      authToken: `Bearer ${this.user?.token}`,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });
    this.hasBaseDropZoneOver = false;
    this.hasAnotherDropZoneOver = false;
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo: Photo = JSON.parse(response);
        this.member.photos.push(photo);
        if (this.member.photos.length === 1) {
          this.updateUserAndMemberPhotoUrl(photo);
        }
      }
    };
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }
  //#endregion

  onSetMainPhoto(photoId: number) {
    this.memberService.setMainPhoto(photoId).subscribe((photo: Photo) => {
      this.updateUserAndMemberPhotoUrl(photo);
    });
  }

  onDeletePhoto(photoId: number) {
    this.memberService.deletePhoto(photoId).subscribe((photo) => {
      //update member
      //this.member.photos = this.member.photos.filter((x) => x.id != photo.id);
      this.member.photos.splice(
        this.member.photos.findIndex((x) => x.id == photo.id),
        1
      );
      this.toastr.warning('تصویر با موفقیت حذف گردید');
    });
  }

  private updateUserAndMemberPhotoUrl(photo: Photo) {
    //update user
    this.user.photoUrl = photo.url;
    this.accountService.setCurrentUser(this.user);
    //update member
    this.member.photoUrl = photo.url;
    this.member.photos.forEach((x) => {
      if (x.isMain) x.isMain = false;
      if (x.id === photo.id) x.isMain = true;
    });
  }
}
