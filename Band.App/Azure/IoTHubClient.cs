using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Band.App.Azure
{
    public class IoTHubClient
    {
        private const string IOT_HUB_URI = "jasoniot.azure-devices.net";
        private const string DEVICE_KEY = "<put key here>";
        private const string DEVICE_ID = "jasonsuno";

        private DeviceClient _Client;

        public IoTHubClient()
        {
            
            _Client = DeviceClient.Create(IOT_HUB_URI, new DeviceAuthenticationWithRegistrySymmetricKey(DEVICE_ID, DEVICE_KEY), TransportType.Http1);

        }


        public Task SendRed()
        {
            return SendMessage("red");
;        }

        public Task SendBlue()
        {
            return SendMessage("blue");
        }

        public Task SendYellow()
        {
            return SendMessage("yellow");
        }

        public Task SendGreen()
        {
            return SendMessage("green");
        }

        private async Task SendMessage(string color)
        {
            var message = new Message(Encoding.ASCII.GetBytes(color));
            await _Client.SendEventAsync(message);
        }
    }
}
