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
        IStream _Connection;

        public event EventHandler<EventArgs> DeviceReady;
        public event EventHandler<ButtonArgs> ButtonPressed;


        public async Task<bool> ConnectToArduino()
        {
            var board = await FindFirstArduinoBoard();
            if (board != null)
            {
                _Connection = new UsbSerial(board);
                _Connection.ConnectionEstablished += () =>
                {
                    Debug.WriteLine("Established");
                };

                _Connection.ConnectionFailed += (m) => 
                {
                    Debug.WriteLine("Failed " + m);
                };
                
                _Arduino = new RemoteDevice(_Connection);
                _Arduino.DeviceReady += _Arduino_DeviceReady;


                _Connection.begin(57600, 0);

                return true;
            }


            return false;
        }


        private void _Arduino_DeviceReady()
        {
            Debug.WriteLine("Device is ready");
            Setup();
            if (DeviceReady != null)
            {
                DeviceReady(this, new EventArgs());
                //Loop();
            }
        }

        private PinState _LastRed = PinState.HIGH;
        private Task Loop()
        {
            return Task.Run(() =>
            {
                while(true)
                { 
                    var red = _Arduino.digitalRead(RED_BUTTON_PIN);
                    if(red != _LastRed)
                    {
                        Debug.WriteLine(red);
                        _LastRed = red;
                    }
                }
            });
        }
        private void _Arduino_DigitalPinUpdated(byte pin, PinState state)
        {
            if (state == PinState.HIGH) 
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
            _Arduino.DigitalPinUpdated += _Arduino_DigitalPinUpdated;
            _Arduino.pinMode(RED_BUTTON_PIN, PinMode.INPUT);
            _Arduino.pinMode(YELLOW_BUTTON_PIN, PinMode.INPUT);
            _Arduino.pinMode(GREEN_BUTTON_PIN, PinMode.INPUT);
            _Arduino.pinMode(BLUE_BUTTON_PIN, PinMode.INPUT);

         
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
