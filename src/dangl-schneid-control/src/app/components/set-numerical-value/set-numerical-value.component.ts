import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-set-numerical-value',
  templateUrl: './set-numerical-value.component.html',
  styleUrls: ['./set-numerical-value.component.scss'],
})
export class SetNumericalValueComponent {
  get canSave(): boolean {
    return (
      this.selectedValue >= this.data.min && this.selectedValue <= this.data.max
    );
  }

  selectedValue: number;

  constructor(
    private matDialogRef: MatDialogRef<SetNumericalValueComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: {
      currentValue: number;
      min: number;
      max: number;
      label: string;
      unit: string;
    }
  ) {
    this.selectedValue = data.currentValue;
  }

  changeValue(): void {
    if (
      this.selectedValue &&
      this.selectedValue >= this.data.min &&
      this.selectedValue <= this.data.max
    ) {
      this.matDialogRef.close(this.selectedValue);
    }
  }

  close(): void {
    this.matDialogRef.close();
  }
}
