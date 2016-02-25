using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDeviceKey
{
    class Program
    {
        static RegistryManager REGISTRY_MANAGER;
        static string CONNECTION_STRING = "<Connection String>";

        static void Main(string[] args)
        {
            REGISTRY_MANAGER = RegistryManager.CreateFromConnectionString(CONNECTION_STRING);

            Console.WriteLine("Enter an id for the device you wish to add...");
            var deviceId = Console.ReadLine();

            AddDeviceAsync(deviceId).Wait();

            Console.ReadLine();
        }

        private async static Task AddDeviceAsync(string deviceId)
        {
            Device device;
            try
            {
                device = await REGISTRY_MANAGER.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await REGISTRY_MANAGER.GetDeviceAsync(deviceId);
            }
            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }
    }
}
