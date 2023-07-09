import { Component, EventEmitter, Input, Output } from '@angular/core';

import { DecimalValue } from '../../generated-client/generated-client';

@Component({
  selector: 'app-decimal-value',
  templateUrl: './decimal-value.component.html',
  styleUrls: ['./decimal-value.component.scss'],
})
export class DecimalValueComponent {
  @Input() label: string | null = null;
  @Input() canEdit = false;
  @Input() decimalPlaces: number = 1;
  @Input() value: DecimalValue | null = null;
  @Output() onEditRequested = new EventEmitter<void>();

  get numberFormat(): string {
    return `1.${this.decimalPlaces}-${this.decimalPlaces}`;
  }

  raiseEditEvent(): void {
    this.onEditRequested.emit();
  }
}
