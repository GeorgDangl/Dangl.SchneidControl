import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogContent } from '@angular/material/dialog';
import {
  ConfigurationClient,
  TransferStationStatus,
} from '../../generated-client/generated-client';
import { CdkScrollable } from '@angular/cdk/scrolling';
import { MatSelect, MatOption } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { TransferStationStatusPipe } from '../../pipes/transfer-station-status.pipe';

@Component({
    selector: 'app-set-transfer-station-status',
    templateUrl: './set-transfer-station-status.component.html',
    styleUrls: ['./set-transfer-station-status.component.scss'],
    imports: [CdkScrollable, MatDialogContent, MatSelect, FormsModule, MatOption, MatButton, TransferStationStatusPipe]
})
export class SetTransferStationStatusComponent {
  constructor(
    private matDialogRef: MatDialogRef<SetTransferStationStatusComponent>,
    private configurationClient: ConfigurationClient,
    @Inject(MAT_DIALOG_DATA)
    data: { currentValue: TransferStationStatus }
  ) {
    this.selectedStatus = data.currentValue;
  }

  selectedStatus: TransferStationStatus;
  availableStati = Object.keys(TransferStationStatus).map(
    (key) => key as TransferStationStatus
  );

  changeStatus(): void {
    this.configurationClient
      .setTransferStationMode(this.selectedStatus)
      .subscribe({
        next: () => {
          this.matDialogRef.close(this.selectedStatus);
        },
        error: () => {
          this.matDialogRef.close();
        },
      });
  }

  close(): void {
    this.matDialogRef.close();
  }
}
