import { Component, Input } from '@angular/core';

import { DecimalValue } from '../../generated-client/generated-client';

@Component({
  selector: 'app-decimal-value',
  templateUrl: './decimal-value.component.html',
  styleUrls: ['./decimal-value.component.scss'],
})
export class DecimalValueComponent {
  @Input() label: string | null = null;
  @Input() decimalPlaces: number = 1;
  @Input() value: DecimalValue | null = null;

  get numberFormat(): string {
    return `1.${this.decimalPlaces}-${this.decimalPlaces}`;
  }
}
