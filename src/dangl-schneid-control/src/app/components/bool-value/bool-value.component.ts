import { Component, EventEmitter, Input, Output } from '@angular/core';

import { BoolValue } from '../../generated-client/generated-client';
import { MatCard, MatCardHeader, MatCardTitle, MatCardContent, MatCardActions } from '@angular/material/card';
import { NgClass } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'app-bool-value',
    templateUrl: './bool-value.component.html',
    styleUrls: ['./bool-value.component.scss'],
    imports: [MatCard, NgClass, MatCardHeader, MatCardTitle, MatCardContent, MatCardActions, MatButton, MatIcon]
})
export class BoolValueComponent {
  @Input() label: string | null = null;
  @Input() value: BoolValue | null = null;
  @Input() canShowStats = false;
  @Input() isActive = false;
  @Output() onStatsRequested = new EventEmitter<void>();

  showStatsEvent(): void {
    this.onStatsRequested.emit();
  }
}
