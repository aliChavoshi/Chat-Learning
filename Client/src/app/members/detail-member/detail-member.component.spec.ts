import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailMemberComponent } from './detail-member.component';

describe('DetailMemberComponent', () => {
  let component: DetailMemberComponent;
  let fixture: ComponentFixture<DetailMemberComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DetailMemberComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DetailMemberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
