import { Component, Inject, OnInit } from '@angular/core';
import {
  LogEntryType,
  Stats,
  StatsClient,
} from '../../generated-client/generated-client';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Moment } from 'moment';
import { Subscription } from 'rxjs';

import * as moment from 'moment';

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.scss'],
})
export class StatsComponent implements OnInit {
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

  constructor(
    private matDialog: MatDialogRef<StatsComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: {
      logEntryType: LogEntryType;
      label: string;
    },
    private statsClient: StatsClient
  ) {
    this.label = data.label;
  }

  ngOnInit(): void {
    this.queryData();
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
      .getStats(fromDate, endDate, this.data.logEntryType)
      .subscribe((stats) => {
        this.unit = stats.unit;
        this.dataSet = [
          {
            name: `${this.label} (${this.unit})`,
            series: stats.entries.map((e) => {
              return {
                name: moment(e.createdAtUtc).format('YYYY-MM-DD HH:mm'),
                value: e.value,
              };
            }),
          },
        ];
      });
  }
}
