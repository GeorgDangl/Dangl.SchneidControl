import { APP_BASE_HREF } from '@angular/common';
import { AngularMaterialSharedModule } from '@dangl/angular-material-shared';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { DecimalValueComponent } from './components/decimal-value/decimal-value.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { NgModule } from '@angular/core';
import { BoolValueComponent } from './components/bool-value/bool-value.component';
import { TransferStationStatusComponent } from './components/transfer-station-status/transfer-station-status.component';
import { TransferStationStatusPipe } from './pipes/transfer-station-status.pipe';

@NgModule({
  declarations: [AppComponent, DecimalValueComponent, BoolValueComponent, TransferStationStatusComponent, TransferStationStatusPipe],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    AngularMaterialSharedModule,
    HttpClientModule,
    MatButtonModule,
    MatProgressBarModule,
    MatSelectModule,
    FormsModule,
    MatInputModule,
    MatCardModule,
    MatProgressSpinnerModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
