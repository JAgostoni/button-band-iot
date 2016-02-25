using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;

namespace Band.App.Arduino
{
    public class ArduinoButtons
    {

        private const byte RED_BUTTON_PIN = 5;
        private const byte YELLOW_BUTTON_PIN = 4;
        private const byte GREEN_BUTTON_PIN = 3;
        private const byte BLUE_BUTTON_PIN = 2;

        RemoteDevice _Arduino;


        public event EventHandler<EventArgs> DeviceReady;
        public event EventHandler<ButtonArgs> ButtonPressed;


        public async Task<bool> ConnectToArduino()
        {
            var board = await FindFirstArduinoBoard();
            if (board != null)
            {
                var port = new UsbSerial(board);
                port.ConnectionEstablished += () =>
                {
                    Debug.WriteLine("Established");
                };

                port.ConnectionFailed += (m) => 
                {
                    Debug.WriteLine("Failed " + m);
                };
                
                _Arduino = new RemoteDevice(port);
                _Arduino.DeviceReady += _Arduino_DeviceReady;
                _Arduino.DigitalPinUpdated += _Arduino_DigitalPinUpdated;


                port.begin(57600, SerialConfig.SERIAL_8N1);

                return true;
            }


            return false;
        }


        private void _Arduino_DeviceReady()
        {
            Setup();
            if (DeviceReady != null)
            {
                DeviceReady(this, new EventArgs());
            }
        }

        private void _Arduino_DigitalPinUpdated(byte pin, PinState state)
        {
            if (state == PinState.LOW) 
            {
                ButtonColor color;
                switch (pin)
                {
                    case RED_BUTTON_PIN:
                        color = ButtonColor.Red;
                        break;
                    case YELLOW_BUTTON_PIN:
                        color = ButtonColor.Yellow;
                        break;
                    case GREEN_BUTTON_PIN:
                        color = ButtonColor.Green;
                        break;
                    case BLUE_BUTTON_PIN:
                        color = ButtonColor.Blue;
                        break;
                    default:
                        return;
                }

                if (ButtonPressed != null)
                {
                    ButtonPressed(this, new ButtonArgs(color));
                }

            }
        }

        private async Task<DeviceInformation> FindFirstArduinoBoard()
        {
            var usbDevices = await UsbSerial.listAvailableDevicesAsync();
            return usbDevices.FirstOrDefault(d => d.Name.ToUpper().Contains("ARDUINO"));     
        }

        private void Setup ()
        {
            _Arduino.pinMode(RED_BUTTON_PIN, PinMode.PULLUP);
            _Arduino.pinMode(YELLOW_BUTTON_PIN, PinMode.PULLUP);
            _Arduino.pinMode(GREEN_BUTTON_PIN, PinMode.PULLUP);
            _Arduino.pinMode(BLUE_BUTTON_PIN, PinMode.PULLUP);

         
        }

    }


    public enum ButtonColor {  Red, Yellow, Green, Blue }
    public class ButtonArgs : EventArgs
    {
        public ButtonColor Color { get; set; }

        public ButtonArgs(ButtonColor whichButton)
        {
            Color = whichButton;
        }
    }
}
