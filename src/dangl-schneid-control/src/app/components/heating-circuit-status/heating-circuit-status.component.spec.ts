import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeatingCircuitStatusComponent } from './heating-circuit-status.component';
import { SharedTestingModule } from 'src/app/shared-testing.module';

describe('HeatingCircuitStatusComponent', () => {
  let component: HeatingCircuitStatusComponent;
  let fixture: ComponentFixture<HeatingCircuitStatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
    imports: [
        SharedTestingModule,
        HeatingCircuitStatusComponent
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
