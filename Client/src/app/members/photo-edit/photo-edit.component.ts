import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/_services/account.service';
import { User } from 'src/app/_models/account';
import { environment } from 'src/environments/environment';
import { IMember, Photo } from 'src/app/_models/member';
import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';

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

  ngOnInit(): void {
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.user = user));
    this.initializeUploader();
  }

  constructor(private accountService: AccountService) {}

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
}
