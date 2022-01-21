import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberCardSharedComponent } from './member-card-shared.component';

describe('MemberCardSharedComponent', () => {
  let component: MemberCardSharedComponent;
  let fixture: ComponentFixture<MemberCardSharedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MemberCardSharedComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MemberCardSharedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
