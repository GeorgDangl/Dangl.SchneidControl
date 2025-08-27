import { Component, Input } from '@angular/core';

import { EnumValueOfHeatingCircuitStatus } from '../../generated-client/generated-client';
import { MatCard, MatCardHeader, MatCardTitle, MatCardContent } from '@angular/material/card';
import { HeatingCircuitStatusPipe } from '../../pipes/heating-circuit-status.pipe';

@Component({
    selector: 'app-heating-circuit-status',
    templateUrl: './heating-circuit-status.component.html',
    styleUrls: ['./heating-circuit-status.component.scss'],
    imports: [MatCard, MatCardHeader, MatCardTitle, MatCardContent, HeatingCircuitStatusPipe]
})
export class HeatingCircuitStatusComponent {
  @Input() label: string | null = null;
  @Input() value: EnumValueOfHeatingCircuitStatus | null = null;
}
