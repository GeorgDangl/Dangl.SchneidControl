import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetTransferStationStatusComponent } from './set-transfer-station-status.component';
import { SharedTestingModule } from 'src/app/shared-testing.module';
import { TransferStationStatusPipe } from 'src/app/pipes/transfer-station-status.pipe';

describe('SetTransferStationStatusComponent', () => {
  let component: SetTransferStationStatusComponent;
  let fixture: ComponentFixture<SetTransferStationStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SetTransferStationStatusComponent, TransferStationStatusPipe],
      imports: [
        SharedTestingModule
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
