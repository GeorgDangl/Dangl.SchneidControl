using System.Net;

namespace Dangl.SchneidControl.Configuration
{
    public partial class SchneidControlSettings
    {
        public string SchneidModbusIpAddress { get; set; }

        public int SchneidModbusTcpPort { get; set; }

        public string DatabaseLoggingFilePath { get; set; }

        public SmtpSettings SmtpSettings { get; set; }

        public List<string> EmailRecipients { get; set; }

        public int MainBufferMaximumTemperature { get; set; }

        public void Validate()
        {
            if (!IPAddress.TryParse(SchneidModbusIpAddress, out var _))
            {
                throw new AppConfigException($"{nameof(SchneidModbusIpAddress)} is not a valid IP address: {SchneidModbusIpAddress}");
            }

            if (SchneidModbusTcpPort < 1 || SchneidModbusTcpPort > 65535)
            {
                throw new AppConfigException($"{nameof(SchneidModbusTcpPort)} is not a valid port: {SchneidModbusTcpPort}");
            }
        }
    }
}
