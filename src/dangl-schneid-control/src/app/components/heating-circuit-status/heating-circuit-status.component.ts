import { Component, Input } from '@angular/core';

import { EnumValueOfHeatingCircuitStatus } from '../../generated-client/generated-client';

@Component({
  selector: 'app-heating-circuit-status',
  templateUrl: './heating-circuit-status.component.html',
  styleUrls: ['./heating-circuit-status.component.scss'],
})
export class HeatingCircuitStatusComponent {
  @Input() label: string | null = null;
  @Input() value: EnumValueOfHeatingCircuitStatus | null = null;
}
