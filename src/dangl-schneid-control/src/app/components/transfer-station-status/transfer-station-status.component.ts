import { Component, EventEmitter, Input, Output } from '@angular/core';
import {
  EnumValueOfTransferStationStatus,
  TransferStationStatus,
} from '../../generated-client/generated-client';

import { MatDialog } from '@angular/material/dialog';
import { SetTransferStationStatusComponent } from '../set-transfer-station-status/set-transfer-station-status.component';

@Component({
  selector: 'app-transfer-station-status',
  templateUrl: './transfer-station-status.component.html',
  styleUrls: ['./transfer-station-status.component.scss'],
})
export class TransferStationStatusComponent {
  @Input() label: string | null = null;
  @Input() value: EnumValueOfTransferStationStatus | null = null;
  @Output() onStatusChanged = new EventEmitter<void>();

  constructor(private dialog: MatDialog) {}

  initiateTransferStationStatusChange(): void {
    if (this.value) {
      this.dialog
        .open(SetTransferStationStatusComponent, {
          data: { currentValue: this.value!.value },
        })
        .afterClosed()
        .subscribe((newStatus?: TransferStationStatus) => {
          if (newStatus) {
            this.onStatusChanged.emit();
          }
        });
    }
  }
}
