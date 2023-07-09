import { Observable, combineLatest, map } from 'rxjs';

import { DashboardValues } from '../models/dashboard-values';
import { Injectable } from '@angular/core';
import { ValuesClient } from '../generated-client/generated-client';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  constructor(private valuesClient: ValuesClient) {}

  public getDashboardValues(): Observable<DashboardValues> {
    const dashboardValuesObservable = combineLatest([
      this.valuesClient.getAdvanceTemperature(),
      this.valuesClient.getBoilerTemperatureBottom(),
      this.valuesClient.getBoilerTemperatureTop(),
      this.valuesClient.getBufferTemperatureBottom(),
      this.valuesClient.getBufferTemperatureTop(),
      this.valuesClient.getHeatingCircuitStatus(0),
      this.valuesClient.getHeatingCircuitStatus(1),
      this.valuesClient.getCurrentHeatingPower(),
      this.valuesClient.getOuterTemperature(),
      this.valuesClient.getPrimaryRefluxTemperature(),
      this.valuesClient.getPumpStatusHeatingCircuit(0),
      this.valuesClient.getPumpStatusHeatingCircuit(1),
      this.valuesClient.getSecondaryRefluxTemperature(),
      this.valuesClient.getTargetBoilerTemperature(),
      this.valuesClient.getTargetBufferTopTemperature(),
      this.valuesClient.getTotalEnergyConsumption(),
      this.valuesClient.getTransferStationStatus(),
      this.valuesClient.getValveOpening(),
    ]).pipe(
      map(
        ([
          advanceTemperature,
          boilerTemperatureBottom,
          boilerTemperatureTop,
          bufferTemperatureBottom,
          bufferTemperatureTop,
          circuitStatus00,
          circuitStatus01,
          heatingPower,
          outerTemperature,
          primaryRefluxTemperature,
          pumpStatus00,
          pumpStatus01,
          secondaryRefluxTemperature,
          targetBoilerTemperature,
          targetBufferTemperature,
          totalEnergyConsumption,
          transferStationStatus,
          valveOpening,
        ]) => {
          const dashboardValues: DashboardValues = {
            advanceTemperature: advanceTemperature,
            boilerTemperatureBottom: boilerTemperatureBottom,
            boilerTemperatureTop: boilerTemperatureTop,
            bufferTemperatureBottom: bufferTemperatureBottom,
            bufferTemperatureTop: bufferTemperatureTop,
            circuitStatus00: circuitStatus00,
            circuitStatus01: circuitStatus01,
            heatingPower: heatingPower,
            outerTemperature: outerTemperature,
            primaryRefluxTemperature: primaryRefluxTemperature,
            pumpStatus00: pumpStatus00,
            pumpStatus01: pumpStatus01,
            secondaryRefluxTemperature: secondaryRefluxTemperature,
            targetBoilerTemperature: targetBoilerTemperature,
            targetBufferTemperature: targetBufferTemperature,
            totalEnergyConsumption: totalEnergyConsumption,
            transferStationStatus: transferStationStatus,
            valveOpening: valveOpening,
          };

          return dashboardValues;
        }
      )
    );

    return dashboardValuesObservable;
  }
}
