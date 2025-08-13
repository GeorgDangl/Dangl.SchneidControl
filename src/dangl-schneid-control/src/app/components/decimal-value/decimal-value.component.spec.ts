import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DecimalValueComponent } from './decimal-value.component';
import { SharedTestingModule } from 'src/app/shared-testing.module';

describe('DecimalValueComponent', () => {
  let component: DecimalValueComponent;
  let fixture: ComponentFixture<DecimalValueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
    imports: [
        SharedTestingModule,
        DecimalValueComponent,
    ]
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
