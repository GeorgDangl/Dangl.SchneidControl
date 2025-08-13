import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetNumericalValueComponent } from './set-numerical-value.component';
import { SharedTestingModule } from 'src/app/shared-testing.module';

describe('SetNumericalValueComponent', () => {
  let component: SetNumericalValueComponent;
  let fixture: ComponentFixture<SetNumericalValueComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
    imports: [
        SharedTestingModule,
        SetNumericalValueComponent
    ]
});
    fixture = TestBed.createComponent(SetNumericalValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
