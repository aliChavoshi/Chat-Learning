import { Subscription } from 'rxjs';
import { MessageService } from './../../_services/message.service';
import { IMessage } from './../../_models/message';
import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
  messages: IMessage[] = [];
  //gallery
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  //tabset
  @ViewChild('staticTabs', { static: true }) staticTabs: TabsetComponent;
  activeTab: TabDirective;
  tabId = 0;

  constructor(
    private route: ActivatedRoute,
    private messageService: MessageService
  ) {}

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
  loadMessageThread() {
    const sub$ = this.messageService
      .getMessageThread(this.member?.userName)
      .subscribe((res) => {
        this.messages = res;
      });
    this.sub?.add(sub$);
  }
  selectTab(tabId: number) {
    this.staticTabs.tabs[tabId].active = true;
  }
  onTabChange(tab: TabDirective) {
    this.activeTab = tab ?? null;
    if (this.activeTab) {
      if (
        this?.activeTab?.heading === 'Messages' &&
        this?.messages?.length === 0
      ) {
        this.loadMessageThread();
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
  }
}
