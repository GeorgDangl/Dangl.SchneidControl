import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeatingCircuitStatusComponent } from './heating-circuit-status.component';
import { SharedTestingModule } from 'src/app/shared-testing.module';
import { HeatingCircuitStatusPipe } from 'src/app/pipes/heating-circuit-status.pipe';

describe('HeatingCircuitStatusComponent', () => {
  let component: HeatingCircuitStatusComponent;
  let fixture: ComponentFixture<HeatingCircuitStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
    imports: [
        SharedTestingModule,
        HeatingCircuitStatusComponent, HeatingCircuitStatusPipe
    ]
});
    fixture = TestBed.createComponent(HeatingCircuitStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
