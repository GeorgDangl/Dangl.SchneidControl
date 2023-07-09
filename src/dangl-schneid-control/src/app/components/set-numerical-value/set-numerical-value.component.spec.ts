import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetNumericalValueComponent } from './set-numerical-value.component';

describe('SetNumericalValueComponent', () => {
  let component: SetNumericalValueComponent;
  let fixture: ComponentFixture<SetNumericalValueComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SetNumericalValueComponent]
    });
    fixture = TestBed.createComponent(SetNumericalValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
