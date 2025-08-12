import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { SharedTestingModule } from './shared-testing.module';
import { HeaderComponent } from '@dangl/angular-material-shared';
import { TransferStationStatusComponent } from './components/transfer-station-status/transfer-station-status.component';
import { HeatingCircuitStatusComponent } from './components/heating-circuit-status/heating-circuit-status.component';
import { BoolValueComponent } from './components/bool-value/bool-value.component';
import { DecimalValueComponent } from './components/decimal-value/decimal-value.component';
import { TransferStationStatusPipe } from './pipes/transfer-station-status.pipe';
import { HeatingCircuitStatusPipe } from './pipes/heating-circuit-status.pipe';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SharedTestingModule, HeaderComponent],
      declarations: [
        AppComponent,
        TransferStationStatusComponent,
        HeatingCircuitStatusComponent,
        BoolValueComponent,
        DecimalValueComponent,
        TransferStationStatusPipe,
        HeatingCircuitStatusPipe
      ],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

});
