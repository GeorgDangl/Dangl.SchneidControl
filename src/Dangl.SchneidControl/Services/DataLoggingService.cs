using Dangl.SchneidControl.Data;
using Dangl.SchneidControl.Models.Services;

namespace Dangl.SchneidControl.Services
{
    public class DataLoggingService : IDataLoggingService
    {
        private readonly DataLoggingContext _context;
        private readonly ISchneidReadRepository _schneidReadRepository;

        public DataLoggingService(DataLoggingContext context,
            ISchneidReadRepository schneidReadRepository)
        {
            _context = context;
            _schneidReadRepository = schneidReadRepository;
        }

        public async Task<ValuesResult> ReadAndSaveValuesAsync()
        {
            var valuesResult = new ValuesResult();
            try
            {
                await PerformAndIgnoreExceptionsAsync(async () =>
                {
                    var outerTemperature = await _schneidReadRepository.GetOuterTemperaturAsync();
                    if (outerTemperature.IsSuccess)
                    {
                        _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.OuterTemperature, Value = Convert.ToInt32(outerTemperature.Value.Value * 10) });
                    }
                });

                await PerformAndIgnoreExceptionsAsync(async () =>
                {
                    var totalEnergyConsumption = await _schneidReadRepository.GetTotalEnergyConsumptionAsync();
                    if (totalEnergyConsumption.IsSuccess)
                    {
                        _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.TotalEnergyConsumption, Value = Convert.ToInt32(totalEnergyConsumption.Value.Value * 10) });
                    }
                });

                await PerformAndIgnoreExceptionsAsync(async () =>
                {
                    var heatingPowerDraw = await _schneidReadRepository.GetCurrentHeatingPowerDrawAsync();
                    if (heatingPowerDraw.IsSuccess)
                    {
                        _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.HeatingPowerDraw, Value = Convert.ToInt32(heatingPowerDraw.Value.Value) });
                    }
                });

                await PerformAndIgnoreExceptionsAsync(async () =>
                {
                    var bufferTemperature = await _schneidReadRepository.GetBufferTemperatureTopAsync();
                    if (bufferTemperature.IsSuccess)
                    {
                        _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.BufferTemperature, Value = Convert.ToInt32(bufferTemperature.Value.Value * 10) });
                        valuesResult.BufferTemperatureTop = bufferTemperature.Value;
                    }
                });

                await PerformAndIgnoreExceptionsAsync(async () =>
                {
                    var boilerTemperature = await _schneidReadRepository.GetBoilerTemperatureTopAsync();
                    if (boilerTemperature.IsSuccess)
                    {
                        _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.BoilerTemperature, Value = Convert.ToInt32(boilerTemperature.Value.Value * 10) });
                    }
                });

                await PerformAndIgnoreExceptionsAsync(async () =>
                {
                    var valveOpening = await _schneidReadRepository.GetValveOpeningAsync();
                    if (valveOpening.IsSuccess)
                    {
                        _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.ValveOpening, Value = Convert.ToInt32(valveOpening.Value.Value) });
                    }
                });

                await PerformAndIgnoreExceptionsAsync(async () =>
                {
                    var heatingCircuitPump1 = await _schneidReadRepository.GetPumpStatusHeatingCircuit00Async();
                    if (heatingCircuitPump1.IsSuccess)
                    {
                        _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.HeatingCircuit1Pump, Value = heatingCircuitPump1.Value.Value ? 1 : 0 });
                    }
                });

                await PerformAndIgnoreExceptionsAsync(async () =>
                {
                    var heatingCircuitPump2 = await _schneidReadRepository.GetPumpStatusHeatingCircuit01Async();
                    if (heatingCircuitPump2.IsSuccess)
                    {
                        _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.HeatingCircuit2Pump, Value = heatingCircuitPump2.Value.Value ? 1 : 0 });
                    }
                });

                await PerformAndIgnoreExceptionsAsync(async () =>
                {
                    var boilerLoadingPump = await _schneidReadRepository.GetBoilerLoadingPumpStatusAsync();
                    if (boilerLoadingPump.IsSuccess)
                    {
                        _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.BoilerLoadingPump, Value = boilerLoadingPump.Value.Value ? 1 : 0 });
                    }
                });

                await PerformAndIgnoreExceptionsAsync(async () =>
                {
                    var bufferLoadingPump = await _schneidReadRepository.GetBufferLoadingPumpStatusAsync();
                    if (bufferLoadingPump.IsSuccess)
                    {
                        _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.BufferLoadingPump, Value = bufferLoadingPump.Value.Value ? 1 : 0 });
                    }
                });

                await PerformAndIgnoreExceptionsAsync(async () =>
                {
                    var advanceTemperature = await _schneidReadRepository.GetAdvanceTemperatureAsync();
                    if (advanceTemperature.IsSuccess)
                    {
                        _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.AdvanceTemperature, Value = Convert.ToInt32(advanceTemperature.Value.Value * 10) });
                    }
                });

                await PerformAndIgnoreExceptionsAsync(async () =>
                {
                    var advanceTemperature = await _schneidReadRepository.GetHeatingCircuit01AdvanceTemperatureAsync();
                    if (advanceTemperature.IsSuccess)
                    {
                        _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.HeatingCircuit1AdvanceTemperature, Value = Convert.ToInt32(advanceTemperature.Value.Value * 10) });
                    }
                });

                await PerformAndIgnoreExceptionsAsync(async () =>
                {
                    var advanceTemperature = await _schneidReadRepository.GetHeatingCircuit02AdvanceTemperatureAsync();
                    if (advanceTemperature.IsSuccess)
                    {
                        _context.DataEntries.Add(new DataEntry { CreatedAtUtc = DateTime.UtcNow, LogEntryType = LogEntryType.HeatingCircuit2AdvanceTemperature, Value = Convert.ToInt32(advanceTemperature.Value.Value * 10) });
                    }
                });

                await PerformAndIgnoreExceptionsAsync(async () =>
                {
                    var transferStationStatus = await _schneidReadRepository.GetTransferStationStatusAsync();
                    if (transferStationStatus.IsSuccess)
                    {
                        valuesResult.TransferStationStatus = transferStationStatus.Value;
                    }
                });

                await _context.SaveChangesAsync();
            }
            catch
            {
                // We're just ignoring failures here, don't want the task to crash
            }

            return valuesResult;
        }

        private async Task PerformAndIgnoreExceptionsAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch
            {
                // We're just ignoring failures here, don't want the task to crash
            }
        }
    }
}
