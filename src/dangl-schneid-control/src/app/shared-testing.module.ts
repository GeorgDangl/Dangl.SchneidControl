import {
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
  MatMomentDateModule,
} from '@angular/material-moment-adapter';
import {
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgModule } from '@angular/core';
import { provideHttpClientTesting } from '@angular/common/http/testing';

@NgModule({
  imports: [
    AppRoutingModule,
    MatMomentDateModule,
  ],
  exports: [
    AppRoutingModule,
    MatMomentDateModule,
  ],
  providers: [
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
    { provide: MAT_DIALOG_DATA, useValue: [] },
    provideHttpClient(withInterceptorsFromDi()),
    provideHttpClientTesting(),
  ]
})
export class SharedTestingModule {}
