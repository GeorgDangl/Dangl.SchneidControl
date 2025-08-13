import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoolValueComponent } from './bool-value.component';
import { SharedTestingModule } from 'src/app/shared-testing.module';

describe('BoolValueComponent', () => {
  let component: BoolValueComponent;
  let fixture: ComponentFixture<BoolValueComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
    imports: [
        SharedTestingModule,
        BoolValueComponent
    ]
});
    fixture = TestBed.createComponent(BoolValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
