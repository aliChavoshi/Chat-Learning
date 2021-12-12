import { TestBed } from '@angular/core/testing';

import { UniqueUserNameService } from './unique-user-name.service';

describe('UniqueUserNameService', () => {
  let service: UniqueUserNameService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UniqueUserNameService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
