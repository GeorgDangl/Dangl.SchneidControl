import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransferStationStatusComponent } from './transfer-station-status.component';
import { SharedTestingModule } from 'src/app/shared-testing.module';
import { TransferStationStatusPipe } from 'src/app/pipes/transfer-station-status.pipe';

describe('TransferStationStatusComponent', () => {
  let component: TransferStationStatusComponent;
  let fixture: ComponentFixture<TransferStationStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TransferStationStatusComponent, TransferStationStatusPipe],
      imports: [
        SharedTestingModule
      ]
    });
    fixture = TestBed.createComponent(TransferStationStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
