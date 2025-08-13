import { Component, OnInit, inject } from '@angular/core';
import {
  FileResponse,
  LogEntryType,
  Stats,
  StatsClient,
} from '../../generated-client/generated-client';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Moment } from 'moment';
import { Subscription } from 'rxjs';

import moment from 'moment';
import { saveAs } from '../../utilities/file-save';
import { MatFormField, MatSuffix } from '@angular/material/select';
import { MatInput } from '@angular/material/input';
import { MatDatepickerInput, MatDatepickerToggle, MatDatepicker } from '@angular/material/datepicker';
import { FormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { AreaChartModule } from '@swimlane/ngx-charts';

@Component({
    selector: 'app-stats',
    templateUrl: './stats.component.html',
    styleUrls: ['./stats.component.scss'],
    imports: [MatFormField, MatInput, MatDatepickerInput, FormsModule, MatDatepickerToggle, MatSuffix, MatDatepicker, MatButton, MatIcon, AreaChartModule]
})
export class StatsComponent implements OnInit {
  private matDialog = inject<MatDialogRef<StatsComponent>>(MatDialogRef);
  data = inject<{
    logEntryType: LogEntryType;
    label: string;
}>(MAT_DIALOG_DATA);
  private statsClient = inject(StatsClient);

  colorScheme = {
    domain: ['#00acc1'],
  };

  private _fromDate: Moment | null = moment().utc().add(-1, 'month');
  set fromDate(value: Moment | null) {
    this._fromDate = value;
    this.queryData();
  }
  get fromDate() {
    return this._fromDate;
  }

  private _endDate: Moment | null = null;
  set endDate(value: Moment | null) {
    this._endDate = value;
    this.queryData();
  }
  get endDate() {
    return this._endDate;
  }

  dataSet: {
    name: string;
    series: { name: string | Date; value: number }[];
  }[] = [];
  private lastQuerySubscription: Subscription | null = null;

  endOfToday = moment().endOf('day');
  label: string | null = null;
  unit: string | null = null;

  constructor() {
    const data = this.data;

    this.label = data.label;
  }

  ngOnInit(): void {
    this.queryData();
  }

  private getUserTimeZoneOffset(): number {
    return new Date().getTimezoneOffset();
  }

  private queryData() {
    if (this.lastQuerySubscription) {
      this.lastQuerySubscription.unsubscribe();
    }

    const fromDate = this._fromDate ? this._fromDate.toDate() : undefined;
    const endDate = this._endDate
      ? this._endDate.endOf('day').toDate()
      : undefined;
    this.lastQuerySubscription = this.statsClient
      .getStats(
        fromDate,
        endDate,
        this.data.logEntryType,
        this.getUserTimeZoneOffset()
      )
      .subscribe((stats) => {
        this.unit = stats.unit;
        this.dataSet = [
          {
            name: `${this.label} (${this.unit})`,
            series: stats.entries.map((e) => {
              return {
                name: moment
                  .utc(e.createdAtUtc)
                  .local()
                  .format('YYYY-MM-DD HH:mm'),
                value: e.value,
              };
            }),
          },
        ];
      });
  }

  saveAsExcel(): void {
    this.statsClient
      .getExcel(
        this._fromDate?.toDate(),
        this._endDate?.toDate(),
        this.data.logEntryType,
        this.getUserTimeZoneOffset()
      )
      .subscribe((fileResponse) => {
        this.downloadFile(fileResponse);
      });
  }

  saveAsCsv(): void {
    this.statsClient
      .getCsv(
        this._fromDate?.toDate(),
        this._endDate?.toDate(),
        this.data.logEntryType,
        this.getUserTimeZoneOffset()
      )
      .subscribe((fileResponse) => {
        this.downloadFile(fileResponse);
      });
  }

  private downloadFile(fileResponse: FileResponse): void {
    let fileName = fileResponse.fileName;

    if (fileResponse.headers) {
      const contentDispositionHeaderName = Object.keys(
        fileResponse.headers
      ).find((h) => h.toUpperCase() === 'Content-Disposition'.toUpperCase());
      if (contentDispositionHeaderName) {
        const contentDisposition = fileResponse.headers[
          contentDispositionHeaderName
        ] as string;
        if (
          contentDisposition &&
          contentDisposition.indexOf("filename*=UTF-8''") > -1
        ) {
          const encodedFileName =
            contentDisposition.split("filename*=UTF-8''")[1];
          fileName = decodeURI(encodedFileName);
        }
      }
    }

    saveAs(fileResponse.data, fileName || 'Datei.xlsx');
  }
}
