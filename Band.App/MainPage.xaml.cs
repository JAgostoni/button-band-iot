using System;
using System.Collections.Generic;
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
        public MainPage()
        {
            this.InitializeComponent();


            var duino = new Arduino.ArduinoButtons();
            duino.DeviceReady += Duino_DeviceReady;
            duino.ButtonPressed += Duino_ButtonPressed;
            duino.ConnectToArduino().Wait();
        }

        private void Duino_ButtonPressed(object sender, Arduino.ButtonArgs e)
        {
            
        }

        private void Duino_DeviceReady(object sender, EventArgs e)
        {
            
        }
    }
}
