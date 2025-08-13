import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import {
  EnumValueOfTransferStationStatus,
  TransferStationStatus,
} from '../../generated-client/generated-client';

import { MatDialog } from '@angular/material/dialog';
import { SetTransferStationStatusComponent } from '../set-transfer-station-status/set-transfer-station-status.component';
import { MatCard, MatCardHeader, MatCardTitle, MatCardContent, MatCardActions } from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { TransferStationStatusPipe } from '../../pipes/transfer-station-status.pipe';

@Component({
    selector: 'app-transfer-station-status',
    templateUrl: './transfer-station-status.component.html',
    styleUrls: ['./transfer-station-status.component.scss'],
    imports: [MatCard, MatCardHeader, MatCardTitle, MatCardContent, MatCardActions, MatButton, MatIcon, TransferStationStatusPipe]
})
export class TransferStationStatusComponent {
  private dialog = inject(MatDialog);

  @Input() label: string | null = null;
  @Input() value: EnumValueOfTransferStationStatus | null = null;
  @Output() onStatusChanged = new EventEmitter<void>();

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
