using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dangl.SchneidControl.Configuration
{
    public partial class SchneidControlSettings
    {
        public string SchneidModbusIpAddress { get; set; }

        public int SchneidModbusTcpPort { get; set; }

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
