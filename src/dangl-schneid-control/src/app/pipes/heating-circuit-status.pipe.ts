import { Pipe, PipeTransform } from '@angular/core';

import { HeatingCircuitStatus } from '../generated-client/generated-client';

@Pipe({ name: 'heatingCircuitStatus' })
export class HeatingCircuitStatusPipe implements PipeTransform {
  transform(value?: HeatingCircuitStatus): string {
    if (value) {
      switch (value) {
        case HeatingCircuitStatus.OffOrFrostControl:
          return 'Aus / Frostschutz';
        case HeatingCircuitStatus.Heating:
          return 'Heizen';
        case HeatingCircuitStatus.ResidualHeat:
          return 'Restwärme';
        case HeatingCircuitStatus.Lowering:
          return 'Absenkung';
        case HeatingCircuitStatus.WarmWaterSubordinate:
          return 'Warmwasser Nachrang';
        case HeatingCircuitStatus.FrostControl:
          return 'Frostschutz';
        case HeatingCircuitStatus.Locked:
          return 'Sperre';
        case HeatingCircuitStatus.Manual:
          return 'Handbetrieb';
        case HeatingCircuitStatus.BakeOut:
          return 'Ausheizen';
        case HeatingCircuitStatus.Voltage:
          return '0-10V';
        case HeatingCircuitStatus.Cooling:
          return 'Kühlen';
      }
    }
    return value || '';
  }
}
