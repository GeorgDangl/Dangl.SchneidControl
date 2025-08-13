import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetTransferStationStatusComponent } from './set-transfer-station-status.component';
import { SharedTestingModule } from 'src/app/shared-testing.module';

describe('SetTransferStationStatusComponent', () => {
  let component: SetTransferStationStatusComponent;
  let fixture: ComponentFixture<SetTransferStationStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
    imports: [
        SharedTestingModule,
        SetTransferStationStatusComponent
    ]
});
    fixture = TestBed.createComponent(SetTransferStationStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
