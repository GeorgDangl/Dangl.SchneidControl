import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogContent } from '@angular/material/dialog';
import { CdkScrollable } from '@angular/cdk/scrolling';
import { MatFormField, MatSuffix } from '@angular/material/select';
import { MatInput } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';

@Component({
    selector: 'app-set-numerical-value',
    templateUrl: './set-numerical-value.component.html',
    styleUrls: ['./set-numerical-value.component.scss'],
    imports: [CdkScrollable, MatDialogContent, MatFormField, MatInput, FormsModule, MatSuffix, MatButton]
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
