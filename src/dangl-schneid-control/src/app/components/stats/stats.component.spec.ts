import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatsComponent } from './stats.component';
import { SharedTestingModule } from 'src/app/shared-testing.module';
import { MatDialogRef } from '@angular/material/dialog';

describe('StatsComponent', () => {
  let component: StatsComponent;
  let fixture: ComponentFixture<StatsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StatsComponent],
      imports: [
        SharedTestingModule
      ],
      providers: [
        { provide: MatDialogRef, useValue: {} }
      ]
    });
    fixture = TestBed.createComponent(StatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
