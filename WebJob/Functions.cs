using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebJob
{
    public class Functions
    {
        static string CONNECTION_STRING = "<set connection string>";
        static string IOTHUB_ENDPOINT = "messages/events";
        static EventHubClient EVENTHUB_CLIENT;
        static string WEBAPI_ADDRESS = "http://buttonband.azurewebsites.net";

        [NoAutomaticTrigger]
        public static async Task ProcessEvents(TextWriter log)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(WEBAPI_ADDRESS);
                await client.PostAsJsonAsync<string>("api/ButtonCounts", "red");
            }

            var eventHubClient = EventHubClient.CreateFromConnectionString(CONNECTION_STRING,IOTHUB_ENDPOINT);
            var partitions = eventHubClient.GetRuntimeInformation().PartitionIds;

            foreach(string partition in partitions)
            {
                ReceiveMessagesFromDeviceAsync(eventHubClient, partition);
            }
        }

        private async static Task ReceiveMessagesFromDeviceAsync(EventHubClient eventHubClient, string partition)
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition);
            while (true)
            {
                EventData eventData = await eventHubReceiver.ReceiveAsync();
                
                if (eventData == null) continue;

                string data = Encoding.UTF8.GetString(eventData.GetBytes());



                Console.WriteLine(string.Format("Message received. Partition: {0} Data: '{1}'", partition, data));
            }
        }
    }
}
