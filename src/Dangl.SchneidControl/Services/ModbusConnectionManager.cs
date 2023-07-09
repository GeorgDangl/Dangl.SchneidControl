using NModbus;
using System.Net;
using System.Net.Sockets;

namespace Dangl.SchneidControl.Services
{
    public class ModbusConnectionManager
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
            var rawData = await ReadHoldingRegistersAsync(address, 1);
            var bytes = BitConverter.GetBytes(rawData.FirstOrDefault());
            var signedInteger = BitConverter.ToInt16(bytes, 0);
            var integerValue = Convert.ToInt32(signedInteger);
            return integerValue;
        }

        public async Task<uint> GetInteger32ValueAsync(ushort address)
        {
            var rawData = await ReadHoldingRegistersAsync(address, 2);
            var bytes = BitConverter.GetBytes(rawData[1])
                .Concat(BitConverter.GetBytes(rawData[0]))
                .ToArray();
            var unsignedInteger = BitConverter.ToUInt32(bytes, 0);
            return unsignedInteger;
        }

        private async Task<ushort[]> ReadHoldingRegistersAsync(ushort address, ushort length)
        {
            await _semaphore.WaitAsync();
            try
            {
                var resultWithRetry = await RetryAsync(async () =>
                {
                    using var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    var serverIP = IPAddress.Parse(_ipAddress);
                    var serverFullAddr = new IPEndPoint(serverIP, _port);
                    await sock.ConnectAsync(serverFullAddr);
                    var factory = new ModbusFactory();
                    using IModbusMaster master = factory.CreateMaster(sock);
                    var rawValue = await master.ReadHoldingRegistersAsync(1, address, length);
                    await sock.DisconnectAsync(reuseSocket: true);
                    return rawValue;
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
                    using var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    var serverIP = IPAddress.Parse(_ipAddress);
                    var serverFullAddr = new IPEndPoint(serverIP, _port);
                    await sock.ConnectAsync(serverFullAddr);
                    var factory = new ModbusFactory();
                    using IModbusMaster master = factory.CreateMaster(sock);
                    var originalBytes = BitConverter.GetBytes(value);
                    var unsignedValue = BitConverter.ToUInt16(originalBytes, 0);
                    await master.WriteSingleRegisterAsync(1, address, unsignedValue);
                    await sock.DisconnectAsync(reuseSocket: true);
                    return true;
                }, 30);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private static async Task<T> RetryAsync<T>(Func<Task<T>> action, int maxRetries)
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

                    await Task.Delay(10);
                }
            }

            throw new Exception("Invalid max retries specified.");
        }
    }
}
