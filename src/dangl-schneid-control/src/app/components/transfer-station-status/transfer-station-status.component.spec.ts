import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransferStationStatusComponent } from './transfer-station-status.component';

describe('TransferStationStatusComponent', () => {
  let component: TransferStationStatusComponent;
  let fixture: ComponentFixture<TransferStationStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TransferStationStatusComponent]
    });
    fixture = TestBed.createComponent(TransferStationStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
