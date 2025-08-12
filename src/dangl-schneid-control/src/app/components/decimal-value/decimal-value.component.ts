import { Component, EventEmitter, Input, Output } from '@angular/core';

import { DecimalValue } from '../../generated-client/generated-client';

@Component({
    selector: 'app-decimal-value',
    templateUrl: './decimal-value.component.html',
    styleUrls: ['./decimal-value.component.scss'],
    standalone: false
})
export class DecimalValueComponent {
  @Input() label: string | null = null;
  @Input() canEdit = false;
  @Input() decimalPlaces: number = 1;
  @Input() value: DecimalValue | null = null;
  @Input() isActive = false;
  @Output() onEditRequested = new EventEmitter<void>();
  @Output() onStatsRequested = new EventEmitter<void>();
  @Output() onConsumptionRequested = new EventEmitter<void>();
  @Input() canShowStats = false;
  @Input() canShowConsumption = false;

  get showActions(): boolean {
    return this.canEdit || !!this.value?.value || this.canShowStats;
  }

  get numberFormat(): string {
    return `1.${this.decimalPlaces}-${this.decimalPlaces}`;
  }

  raiseEditEvent(): void {
    this.onEditRequested.emit();
  }

  showStatsEvent(): void {
    this.onStatsRequested.emit();
  }

  showConsumptionEvent(): void {
    this.onConsumptionRequested.emit();
  }
}
