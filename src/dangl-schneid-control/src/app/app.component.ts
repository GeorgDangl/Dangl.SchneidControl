import {
  ConfigurationClient,
  LogEntryType,
  StatusClient,
} from './generated-client/generated-client';

import { Component } from '@angular/core';
import { ConsumptionComponent } from './components/consumption/consumption.component';
import { DashboardService } from './services/dashboard.service';
import { DashboardValues } from './models/dashboard-values';
import { MatDialog } from '@angular/material/dialog';
import { SetNumericalValueComponent } from './components/set-numerical-value/set-numerical-value.component';
import { SettingsComponent } from './components/settings/settings.component';
import { StatsComponent } from './components/stats/stats.component';
import { Subject } from 'rxjs';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    standalone: false
})
export class AppComponent {
  constructor(
    private dashboardService: DashboardService,
    private matDialog: MatDialog,
    private configurationClient: ConfigurationClient,
    private statusClient: StatusClient
  ) {}

  private lastValues: DashboardValues | null = null;
  public dashboardValues$ = new Subject<DashboardValues>();
  isLoading = false;
  statsAreAvailable = false;
  logEntryTypes = LogEntryType;
  visibleStats: LogEntryType | null = null;

  ngOnInit(): void {
    this.setDashboardLoading();
    this.statusClient.getStatus().subscribe((status) => {
      this.statsAreAvailable = status.statsEnabled;
    });
  }

  private setDashboardLoading(): void {
    const timerAction = () => {
      this.loadDashboardValues(() => {
        setTimeout(() => {
          timerAction();
        }, 10000);
      });
    };

    timerAction();
  }

  private loadDashboardValues(actionAfterLoad?: () => void): void {
    this.isLoading = true;
    this.dashboardService.getDashboardValues().subscribe({
      next: (v) => {
        this.lastValues = v;
        this.dashboardValues$.next(v);
        this.isLoading = false;

        if (actionAfterLoad) {
          actionAfterLoad();
        }
      },
      error: () => {
        this.isLoading = false;

        if (actionAfterLoad) {
          actionAfterLoad();
        }
      },
    });
  }

  transferStationStatusChanged(): void {
    this.loadDashboardValues();
  }

  bufferTemperatureEditRequested(): void {
    if (!this.lastValues) {
      return;
    }

    this.matDialog
      .open(SetNumericalValueComponent, {
        data: {
          currentValue: this.lastValues.targetBufferTemperature.value,
          min: 50,
          max: 70,
          label: 'Pufferspeicher Soll',
          unit: this.lastValues.targetBufferTemperature.unit,
        },
      })
      .afterClosed()
      .subscribe((newValue) => {
        if (newValue) {
          this.configurationClient
            .setTargetBufferTopTemperature(newValue)
            .subscribe({
              next: () => {
                this.loadDashboardValues();
              },
            });
        }
      });
  }

  boilerTemperatureEditRequested(): void {
    if (!this.lastValues) {
      return;
    }

    this.matDialog
      .open(SetNumericalValueComponent, {
        data: {
          currentValue: this.lastValues.targetBoilerTemperature.value,
          min: 50,
          max: 65,
          label: 'Warmwasserspeicher Soll',
          unit: this.lastValues.targetBoilerTemperature.unit,
        },
      })
      .afterClosed()
      .subscribe((newValue) => {
        if (newValue) {
          this.configurationClient
            .setTargetBoilerTemperature(newValue)
            .subscribe({
              next: () => {
                this.loadDashboardValues();
              },
            });
        }
      });
  }

  showStats(type: LogEntryType, label: string): void {
    this.visibleStats = type;
    this.matDialog
      .open(StatsComponent, {
        data: {
          logEntryType: type,
          label: label,
        },
        panelClass: 'stats-dialog',
      })
      .afterClosed()
      .subscribe(() => {
        this.visibleStats = null;
      });
  }

  showConsumption(): void {
    this.matDialog.open(ConsumptionComponent, {
      panelClass: 'stats-dialog',
    });
  }

  openSettings(): void {
    // We want to open the settings component
    this.matDialog.open(SettingsComponent, {
      panelClass: 'settings-dialog',
    });
  }
}
