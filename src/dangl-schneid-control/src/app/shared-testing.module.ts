import {
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
  MatMomentDateModule,
} from '@angular/material-moment-adapter';

import { AppRoutingModule } from './app-routing.module';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { HeaderComponent } from '@dangl/angular-material-shared';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
    declarations: [
    ],
    imports: [
      AppRoutingModule,
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
      HeaderComponent,
      NoopAnimationsModule
    ],
    exports: [
      AppRoutingModule,
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
      HeaderComponent,
      NoopAnimationsModule
    ],
    providers: [
        { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
        { provide: MatDialogRef, useValue: {} },
        { provide: MAT_DIALOG_DATA, useValue: [] },
        provideHttpClient(withInterceptorsFromDi()),
    ],
  })
export class SharedTestingModule {}
