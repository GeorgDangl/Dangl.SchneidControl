import {
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
  MatMomentDateModule,
} from '@angular/material-moment-adapter';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { BoolValueComponent } from './components/bool-value/bool-value.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { ConsumptionComponent } from './components/consumption/consumption.component';
import { DecimalValueComponent } from './components/decimal-value/decimal-value.component';
import { FormsModule } from '@angular/forms';
import { HeatingCircuitStatusComponent } from './components/heating-circuit-status/heating-circuit-status.component';
import { HeatingCircuitStatusPipe } from './pipes/heating-circuit-status.pipe';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { NgModule } from '@angular/core';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { SetNumericalValueComponent } from './components/set-numerical-value/set-numerical-value.component';
import { SetTransferStationStatusComponent } from './components/set-transfer-station-status/set-transfer-station-status.component';
import { StatsComponent } from './components/stats/stats.component';
import { TransferStationStatusComponent } from './components/transfer-station-status/transfer-station-status.component';
import { TransferStationStatusPipe } from './pipes/transfer-station-status.pipe';
import { HeaderComponent } from '@dangl/angular-material-shared';

@NgModule({
    declarations: [
        AppComponent,
        DecimalValueComponent,
        BoolValueComponent,
        TransferStationStatusComponent,
        TransferStationStatusPipe,
        HeatingCircuitStatusPipe,
        HeatingCircuitStatusComponent,
        SetTransferStationStatusComponent,
        SetNumericalValueComponent,
        StatsComponent,
        ConsumptionComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        MatButtonModule,
        MatProgressBarModule,
        MatSelectModule,
        FormsModule,
        MatInputModule,
        MatCardModule,
        MatProgressSpinnerModule,
        MatIconModule,
        MatDialogModule,
        NgxChartsModule,
        MatDatepickerModule,
        MatMomentDateModule,
        HeaderComponent
    ],
    providers: [
        { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
        provideHttpClient(withInterceptorsFromDi()),
    ],
    bootstrap: [AppComponent],
  })
export class AppModule {}
