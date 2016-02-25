using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceSimulator
{
    class Program
    {
        static DeviceClient deviceClient;
        static string IOT_HUB_URI = "jasoniot.azure-devices.net";
        static string DEVICE_KEY = "<ask geoff>";

        static void Main(string[] args)
        {
            deviceClient = DeviceClient.Create(IOT_HUB_URI, new DeviceAuthenticationWithRegistrySymmetricKey("testdevice", DEVICE_KEY));
            SendMessage(deviceClient, "red").Wait();
            SendMessage(deviceClient, "blue").Wait();
            SendMessage(deviceClient, "green").Wait();
            SendMessage(deviceClient, "yellow").Wait();
        }

        public static async Task SendMessage(DeviceClient deviceClient, string color)
        {   

            var message = new Message(Encoding.ASCII.GetBytes(color));
            await deviceClient.SendEventAsync(message);
        }
    }
}
