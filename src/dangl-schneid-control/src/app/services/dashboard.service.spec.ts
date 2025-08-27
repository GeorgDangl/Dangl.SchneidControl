import { TestBed } from '@angular/core/testing';

import { DashboardService } from './dashboard.service';
import { SharedTestingModule } from '../shared-testing.module';

describe('DashboardService', () => {
  let service: DashboardService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        SharedTestingModule
      ]
    });
    service = TestBed.inject(DashboardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
