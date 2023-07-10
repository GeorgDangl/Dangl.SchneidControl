import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DecimalValueComponent } from './decimal-value.component';

describe('DecimalValueComponent', () => {
  let component: DecimalValueComponent;
  let fixture: ComponentFixture<DecimalValueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DecimalValueComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DecimalValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
