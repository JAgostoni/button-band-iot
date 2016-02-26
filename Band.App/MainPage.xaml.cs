using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using System;

namespace Band.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Arduino.ArduinoButtons _Arduino;
        Azure.IoTHubClient _Client;

        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            _Arduino = new Arduino.ArduinoButtons();
            _Arduino.DeviceReady += _Arduino_DeviceReady;
            _Arduino.ButtonPressed += _Arduino_ButtonPressed;
            _Arduino.ConnectToArduino();


            _Client = new Azure.IoTHubClient();

            // Get results from Azure and populate the current button counts
            BlueCount.Text = "0";
            GreenCount.Text = "0";
            YellowCount.Text = "0";
            RedCount.Text = "0";


            
        }

        private void _Arduino_ButtonPressed(object sender, Arduino.ButtonArgs e)
        {
            Debug.WriteLine(e.Color.ToString());


            ButtonPressed(e.Color.ToString());
            
            switch (e.Color)
            {
                case Arduino.ButtonColor.Red:
                    _Client.SendRed();
                    break;
                case Arduino.ButtonColor.Yellow:
                    _Client.SendYellow();
                    break;
                case Arduino.ButtonColor.Green:
                    _Client.SendGreen();
                    break;
                case Arduino.ButtonColor.Blue:
                    _Client.SendBlue();
                    break;
                default:
                    break;
            }
        }

        private void _Arduino_DeviceReady(object sender, System.EventArgs e)
        {
            
        }

        /// <summary>
        /// Call this with a title case color like Blue, Green, Yellow, Red.
        /// </summary>
        /// <param name="buttonColorName"></param>
        private async Task ButtonPressed(string buttonColorName)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => VisualStateManager.GoToState(this, $"{buttonColorName}Pressed", false));

        }
    }
}
