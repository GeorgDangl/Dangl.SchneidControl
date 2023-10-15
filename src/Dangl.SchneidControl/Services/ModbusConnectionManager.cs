using System.Net;
using FluentModbus;

namespace Dangl.SchneidControl.Services
{
    public class ModbusConnectionManager : IDisposable
    {
        private readonly string _ipAddress;
        private readonly int _port;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public ModbusConnectionManager(string ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
        }

        public Task SetInteger16ValueAsync(ushort address, short value)
        {
            return PresetSingleRegisterAsync(address, value);
        }

        public async Task<int> GetInteger16ValueAsync(ushort address)
        {
            var bytes = await ReadHoldingRegistersAsync(address, 1);
            var signedInteger = BitConverter.ToInt16(bytes.Reverse().ToArray(), 0);
            var integerValue = Convert.ToInt32(signedInteger);
            return integerValue;
        }

        public async Task<uint> GetInteger32ValueAsync(ushort address)
        {
            var bytes = await ReadHoldingRegistersAsync(address, 2);
            var unsignedInteger = BitConverter.ToUInt32(bytes.Reverse().ToArray(), 0);
            return unsignedInteger;
        }

        private async Task<byte[]> ReadHoldingRegistersAsync(ushort address, ushort length)
        {
            await _semaphore.WaitAsync();
            try
            {
                var resultWithRetry = await RetryAsync(async () =>
                {
                    var client = GetModbusClient();
                    var ctsSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                    var readBytes = (await client.ReadHoldingRegistersAsync(1, address, length, ctsSource.Token)).ToArray();
                    return readBytes;
                }, 30);

                return resultWithRetry;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task PresetSingleRegisterAsync(ushort address, short value)
        {
            await _semaphore.WaitAsync();
            try
            {
                var resultWithRetry = await RetryAsync(async () =>
                {
                    var client = GetModbusClient();
                    var byteValue = BitConverter.GetBytes(value).Reverse().ToArray();
                    var ctsSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                    await client.WriteSingleRegisterAsync(1, address, byteValue, ctsSource.Token);
                    return true;
                }, 30);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task<T> RetryAsync<T>(Func<Task<T>> action, int maxRetries)
        {
            var retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    var result = await action();
                    return result;
                }
                catch
                {
                    retries++;
                    if (retries >= maxRetries)
                    {
                        throw;
                    }

                    ResetModbusClient();
                    await Task.Delay(10);
                }
            }

            throw new Exception("Invalid max retries specified.");
        }

        private void ResetModbusClient()
        {
            if (_client != null)
            {
                try
                {
                    _client.Disconnect();
                }
                catch
                {
                    // Ignore any errors here
                }
                try
                {
                    _client.Dispose();
                }
                catch
                {
                    // Ignore any errors here
                }
            }

            _client = null;
        }

        public void Dispose()
        {
            ResetModbusClient();
        }

        private ModbusTcpClient? _client;

        private ModbusTcpClient GetModbusClient()
        {
            if (_client == null)
            {
                _client = new ModbusTcpClient();
                _client.Connect(new IPEndPoint(IPAddress.Parse(_ipAddress), _port));
            }

            return _client;
        }
    }
}
