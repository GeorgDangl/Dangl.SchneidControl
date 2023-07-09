import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoolValueComponent } from './bool-value.component';

describe('BoolValueComponent', () => {
  let component: BoolValueComponent;
  let fixture: ComponentFixture<BoolValueComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BoolValueComponent]
    });
    fixture = TestBed.createComponent(BoolValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
