import {
  BoolValue,
  DecimalValue,
  EnumValueOfHeatingCircuitStatus,
  EnumValueOfTransferStationStatus,
} from '../generated-client/generated-client';

export interface DashboardValues {
  outerTemperature: DecimalValue;
  totalEnergyConsumption: DecimalValue;
  heatingPower: DecimalValue;
  advanceTemperature: DecimalValue;
  primaryRefluxTemperature: DecimalValue;
  secondaryRefluxTemperature: DecimalValue;
  valveOpening: DecimalValue;
  bufferTemperatureTop: DecimalValue;
  bufferTemperatureBottom: DecimalValue;
  boilerTemperatureTop: DecimalValue;
  boilerTemperatureBottom: DecimalValue;
  targetBufferTemperature: DecimalValue;
  targetBoilerTemperature: DecimalValue;
  transferStationStatus: EnumValueOfTransferStationStatus;
  circuitStatus00: EnumValueOfHeatingCircuitStatus;
  circuitStatus01: EnumValueOfHeatingCircuitStatus;
  pumpStatus00: BoolValue;
  pumpStatus01: BoolValue;
}
