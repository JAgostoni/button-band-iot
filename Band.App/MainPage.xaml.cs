using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Band.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Arduino.ArduinoButtons _Board;
        Azure.IoTHubClient _Client;

        public MainPage()
        {
            this.InitializeComponent();
            _Client = new Azure.IoTHubClient();

        }

        private async void Duino_ButtonPressed(object sender, Arduino.ButtonArgs e)
        {
            Debug.Write(e.Color.ToString());
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

        private void Duino_DeviceReady(object sender, EventArgs e)
        {
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var _Board = new Arduino.ArduinoButtons();
            _Board.DeviceReady += Duino_DeviceReady;
            _Board.ButtonPressed += Duino_ButtonPressed;
            await _Board.ConnectToArduino();

        }
    }
}
