import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetNumericalValueComponent } from './set-numerical-value.component';
import { SharedTestingModule } from 'src/app/shared-testing.module';
import { MatDialogRef } from '@angular/material/dialog';

describe('SetNumericalValueComponent', () => {
  let component: SetNumericalValueComponent;
  let fixture: ComponentFixture<SetNumericalValueComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SetNumericalValueComponent],
      imports: [
        SharedTestingModule
      ],
      providers: [
        { provide: MatDialogRef, useValue: {} }
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
