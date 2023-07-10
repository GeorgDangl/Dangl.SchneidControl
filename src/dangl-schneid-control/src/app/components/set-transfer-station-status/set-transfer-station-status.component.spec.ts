import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetTransferStationStatusComponent } from './set-transfer-station-status.component';

describe('SetTransferStationStatusComponent', () => {
  let component: SetTransferStationStatusComponent;
  let fixture: ComponentFixture<SetTransferStationStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SetTransferStationStatusComponent]
    });
    fixture = TestBed.createComponent(SetTransferStationStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
