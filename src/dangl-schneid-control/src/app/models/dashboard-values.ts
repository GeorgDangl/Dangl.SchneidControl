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
  circuitStatus01: EnumValueOfHeatingCircuitStatus;
  circuitStatus02: EnumValueOfHeatingCircuitStatus;
  pumpStatus01: BoolValue;
  pumpStatus02: BoolValue;
  boilerLoadingPumpStatus: BoolValue;
  bufferLoadingPumpStatus: BoolValue;
}
