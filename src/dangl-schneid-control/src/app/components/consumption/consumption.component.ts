import moment from 'moment';

import { Component, OnInit } from '@angular/core';
import {
  ConsumptionClient,
  ConsumptionResolution,
} from '../../generated-client/generated-client';

import { MatDialogRef } from '@angular/material/dialog';
import { Moment } from 'moment';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-stats',
    templateUrl: './consumption.component.html',
    styleUrls: ['./consumption.component.scss'],
    standalone: false
})
export class ConsumptionComponent implements OnInit {
  colorScheme = {
    domain: ['#00acc1'],
  };

  private _fromDate: Moment | null = null;
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
  unit: string | null = null;

  private _resolution: ConsumptionResolution = ConsumptionResolution.Monthly;
  set resolution(value: ConsumptionResolution) {
    this._resolution = value;
    this.queryData();
  }
  get resolution() {
    return this._resolution;
  }

  constructor(
    private matDialog: MatDialogRef<ConsumptionComponent>,
    private consumptionClient: ConsumptionClient
  ) {}

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

    let format = '';
    switch (this.resolution) {
      case ConsumptionResolution.Hourly:
        format = 'YYYY-MM-DD HH:00';
        break;
      case ConsumptionResolution.Daily:
        format = 'YYYY-MM-DD';
        break;
      case ConsumptionResolution.Monthly:
        format = 'YYYY-MM';
        break;
      case ConsumptionResolution.Yearly:
        format = 'YYYY';
        break;
      case ConsumptionResolution.Weekly:
        format = 'YYYY';
        break;
    }

    this.lastQuerySubscription = this.consumptionClient
      .getStats(
        fromDate,
        endDate,
        this.resolution,
        this.getUserTimeZoneOffset()
      )
      .subscribe((stats) => {
        this.unit = stats.unit;
        this.dataSet = [
          {
            name: `Energieverbrauch (${this.unit})`,
            series: stats.entries.map((e) => {
              return {
                name:
                  this.resolution === ConsumptionResolution.Weekly
                    ? `${moment
                        .utc(e.createdAtUtc)
                        .local()
                        .format(format)} KW${moment
                        .utc(e.createdAtUtc)
                        .local()
                        .isoWeek()}`
                    : moment.utc(e.createdAtUtc).local().format(format),
                value: e.value,
              };
            }),
          },
        ];
      });
  }
}
