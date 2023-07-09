import { Subject, timer } from 'rxjs';

import { Component } from '@angular/core';
import { DashboardService } from './services/dashboard.service';
import { DashboardValues } from './models/dashboard-values';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  constructor(private dashboardService: DashboardService) {}

  public dashboardValues$ = new Subject<DashboardValues>();
  isLoading = false;

  ngOnInit(): void {
    this.setDashboardLoading();
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
    this.dashboardService.getDashboardValues().subscribe((v) => {
      this.dashboardValues$.next(v);
      this.isLoading = false;

      if (actionAfterLoad) {
        actionAfterLoad();
      }
    });
  }
}
