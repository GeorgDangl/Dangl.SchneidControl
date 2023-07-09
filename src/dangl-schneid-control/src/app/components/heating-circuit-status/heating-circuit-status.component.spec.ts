import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeatingCircuitStatusComponent } from './heating-circuit-status.component';

describe('HeatingCircuitStatusComponent', () => {
  let component: HeatingCircuitStatusComponent;
  let fixture: ComponentFixture<HeatingCircuitStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HeatingCircuitStatusComponent]
    });
    fixture = TestBed.createComponent(HeatingCircuitStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
