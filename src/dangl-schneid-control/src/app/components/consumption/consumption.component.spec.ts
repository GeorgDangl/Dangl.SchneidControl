import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsumptionComponent } from './consumption.component';
import { SharedTestingModule } from 'src/app/shared-testing.module';

describe('ConsumptionComponent', () => {
  let component: ConsumptionComponent;
  let fixture: ComponentFixture<ConsumptionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ConsumptionComponent],
      imports: [
        SharedTestingModule
      ]
    });
    fixture = TestBed.createComponent(ConsumptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
