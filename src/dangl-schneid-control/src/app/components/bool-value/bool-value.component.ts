import { Component, EventEmitter, Input, Output } from '@angular/core';

import { BoolValue } from '../../generated-client/generated-client';

@Component({
  selector: 'app-bool-value',
  templateUrl: './bool-value.component.html',
  styleUrls: ['./bool-value.component.scss'],
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
