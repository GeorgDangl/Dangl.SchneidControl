import { Component, Input } from '@angular/core';

import { EnumValueOfTransferStationStatus } from '../../generated-client/generated-client';

@Component({
  selector: 'app-transfer-station-status',
  templateUrl: './transfer-station-status.component.html',
  styleUrls: ['./transfer-station-status.component.scss'],
})
export class TransferStationStatusComponent {
  @Input() label: string | null = null;
  @Input() value: EnumValueOfTransferStationStatus | null = null;
}
