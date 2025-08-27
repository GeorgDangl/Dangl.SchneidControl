import moment from 'moment';

import { Component, OnInit, inject } from '@angular/core';
import {
  HeatMeterReplacementViewModel,
  HeatMeterReplacementsClient,
} from 'src/app/generated-client/generated-client';

import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { Moment } from 'moment';

@Component({
    selector: 'app-settings',
    imports: [
        MatTableModule,
        DatePipe,
        MatButtonModule,
        MatIconModule,
        FormsModule,
        MatInputModule,
        MatDatepickerModule,
    ],
    templateUrl: './settings.component.html',
    styleUrl: './settings.component.scss'
})
export class SettingsComponent implements OnInit {
  private heatMeterReplacementsClient = inject(HeatMeterReplacementsClient);


  public dataSource: HeatMeterReplacementViewModel[] = [];
  showAddNewUi = false;
  newInitialValue: number = 0;
  newOldMeterValue: number = 0;
  newReplacedAtUtc: Moment = moment().utc();
  newReplacedAtTime: string = '';

  ngOnInit(): void {
    this.refreshDataSource();
  }

  deleteEntry(id: number): void {
    // First show a confirmation dialog

    if (confirm('Are you sure you want to delete this entry?')) {
      this.heatMeterReplacementsClient
        .deleteHeatMeterReplacemenets(id)
        .subscribe({
          next: () => {
            this.refreshDataSource();
          },
          error: (err) => {
            console.error('Error deleting heat meter replacement:', err);
          },
        });
    }
  }

  showAddNew(): void {
    this.showAddNewUi = true;
    this.newInitialValue = 0;
    this.newOldMeterValue = 0;
    this.newReplacedAtUtc = moment().utc();
  }

  saveNewEntry(): void {
    // For the replacedAtUtc, we need to combine the date and time
    // The time must be transformed from local time to UTC
    // Assuming newReplacedAtUtc is already in UTC, we just need to set the
    // hours and minutes based on newReplacedAtTime
    const [hours, minutes] = this.newReplacedAtTime.split(':').map(Number);
    this.newReplacedAtUtc = this.newReplacedAtUtc
      .hour(hours)
      .minute(minutes)
      .second(0)
      .millisecond(0);

    const newEntry: HeatMeterReplacementViewModel = {
      id: 0, // This will be set by the server
      initialValue: this.newInitialValue,
      oldMeterValue: this.newOldMeterValue,
      replacedAtUtc: this.newReplacedAtUtc.toISOString() as any,
    };

    this.heatMeterReplacementsClient
      .createHeatMeterReplacemenet(newEntry)
      .subscribe({
        next: () => {
          this.showAddNewUi = false;
          this.refreshDataSource();
        },
        error: (err) => {
          console.error('Error saving new heat meter replacement:', err);
        },
      });
  }

  private refreshDataSource(): void {
    this.heatMeterReplacementsClient.getHeatMeterReplacements().subscribe({
      next: (replacements) => {
        this.dataSource = replacements;
      },
      error: (err) => {
        console.error('Error loading heat meter replacements:', err);
      },
    });
  }
}
