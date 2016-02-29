using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Band.App.Azure
{
    public class BandAPIClient
    {

        public event EventHandler<CountArgs> CountsUpdated;

        private async Task CallAPI()
        {
            HttpClient apiClient = new HttpClient();
            var result = await apiClient.GetAsync(new Uri("http://buttonband.azurewebsites.net/api/buttoncounts"));

            var data = await result.Content.ReadAsStringAsync();
            var args = await JsonConvert.DeserializeObjectAsync<CountArgs>(data);

            if (CountsUpdated != null)
            {
                CountsUpdated(this, args);
            }
        }

        public async Task CheckEveryFewSeconds()
        {
            while(true)
            {
                await CallAPI();
                await Task.Delay(5000);

            }
        }
    }

    public class CountArgs : EventArgs
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public int Yellow { get; set; }
    }
}
