import { IUser } from 'src/app/_models/account';
import { AccountService } from 'src/app/_services/account.service';
import { Subscription, take } from 'rxjs';
import { MessageService } from './../../_services/message.service';
import { IMessage } from './../../_models/message';
import {
  Component,
  OnInit,
  ViewChild,
  OnDestroy,
  ChangeDetectionStrategy,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  NgxGalleryAnimation,
  NgxGalleryImage,
  NgxGalleryOptions,
} from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { IMember } from 'src/app/_models/member';

@Component({
  selector: 'app-detail-member',
  templateUrl: './detail-member.component.html',
  styleUrls: ['./detail-member.component.css'],
})
export class DetailMemberComponent implements OnInit, OnDestroy {
  private sub = new Subscription();
  member: IMember;
  currentUser: IUser;
  //gallery
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  //tabset
  @ViewChild('staticTabs', { static: true }) staticTabs: TabsetComponent;
  activeTab: TabDirective;
  tabId = 0;

  constructor(
    private route: ActivatedRoute,
    private messageService: MessageService,
    private accountService: AccountService,
    private router: Router
  ) {
    const sub$ = this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((res) => {
        this.currentUser = res;
      });
    this.sub?.add(sub$);
    //new instance of subscription
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.loadMember();
    this.loadOptions();
    this.route.queryParams.subscribe((params) => {
      this.tabId = +params['tab'] ?? 0;
      if (params['tab']) {
        this.selectTab(this.tabId);
      }
    });
  }
  loadMember() {
    this.route.data.subscribe((data) => {
      this.member = data['member'] as IMember;
    });
    this.galleryImages = this.getImages();
  }

  selectTab(tabId: number) {
    this.staticTabs.tabs[tabId].active = true;
  }
  onTabChange(tab: TabDirective) {
    this.activeTab = tab ?? null;
    if (this.activeTab) {
      if (this?.activeTab?.heading === 'Messages') {
        //signalR
        this.messageService.createHubConnection(
          this?.currentUser,
          this?.member?.userName
        );
      } else {
        this.messageService.stopHubConnection();
      }
    }
  }
  private loadOptions() {
    this.galleryOptions = [
      {
        width: '400px',
        height: '400px',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false,
      },
    ];
  }
  private getImages() {
    const images: NgxGalleryImage[] = [];
    for (let image of this.member.photos) {
      images.push({
        big: image.url,
        medium: image.url,
        small: image.url,
      });
    }
    return images;
  }
  ngOnDestroy(): void {
    this.sub.unsubscribe();
    this.messageService.stopHubConnection();
  }
}
