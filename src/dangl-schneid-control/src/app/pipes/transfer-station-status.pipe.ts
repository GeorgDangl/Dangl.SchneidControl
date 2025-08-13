import { Pipe, PipeTransform } from '@angular/core';

import { TransferStationStatus } from '../generated-client/generated-client';

@Pipe({
    name: 'transferStationStatus',
    standalone: false
})
export class TransferStationStatusPipe implements PipeTransform {
  transform(value?: TransferStationStatus): string {
    if (value) {
      switch (value) {
        case TransferStationStatus.OffOrFrostControl:
          return 'Aus / Frostschutz';
        case TransferStationStatus.OnlyLowering:
          return 'Nur Absenkbetrieb';
        case TransferStationStatus.OnlyHeating:
          return 'Nur Heizbetrieb';
        case TransferStationStatus.Automatic:
          return 'Automatikbetrieb / Zeitprogramm';
        case TransferStationStatus.OnlyBoiler:
          return 'Nur Boilerbetrieb';
        case TransferStationStatus.PartyMode:
          return 'Partymodus';
        case TransferStationStatus.Maintenance:
          return 'Wartung (Aus, kein Frostschutz)';
      }
    }

    return value || '';
  }
}
