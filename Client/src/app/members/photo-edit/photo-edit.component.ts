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
    private memberService: MemberService
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
      //update user
      this.user.photoUrl = photo.url;
      this.accountService.setCurrentUser(this.user);
      //update member
      this.member.photoUrl = photo.url;
      this.member.photos.forEach((item) => {
        if (item.isMain) {
          item.isMain = false;
        } else if (item.id === photo.id) {
          item.isMain = true;
        }
      });
    });
  }
}
