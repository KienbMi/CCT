using System;
using System.Collections.Generic;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.RaspberryIO.Peripherals;
using Unosquare.WiringPi;

namespace CCT.NfcReaderConsole
{
    class Functions_RC522
    {
        static bool _initDone = false;
        static IGpioPin _blinkingPin;
        static RFIDControllerMfrc522 _nfcReader;

        public static void Init()
        {
            Pi.Init<BootstrapWiringPi>();           
            
            // set IO Pin to Output
            _blinkingPin = Pi.Gpio[17];
            _blinkingPin.PinMode = GpioPinDriveMode.Output;
            Console.WriteLine("** GPIO Initialization done **");

            // init RC522 Reader
            // default constructor uses the BCM22 pin
            _nfcReader = new RFIDControllerMfrc522();
            Console.WriteLine("** RC522 Initialization done **");

            _initDone = true;
            Console.WriteLine("\n**** Raspberry Informations: ****\n");
            Console.WriteLine(Pi.Info);
        }

        public static void InvertLedSignal()
        {
            if (_initDone == false)
                Init();

            var isOn = _blinkingPin.Read();
            if (isOn)
            {
                _blinkingPin.Write(false);
            }
            else
            {
                _blinkingPin.Write(true);
            }
        }

        public static string ReadTagFromRC522()
        {
            if (_initDone == false)
                Init();

            string result = string.Empty;

            if (_nfcReader.DetectCard() == RFIDControllerMfrc522.Status.AllOk)
            {
                var uidResponse = _nfcReader.ReadCardUniqueId();
                if (uidResponse.Status == RFIDControllerMfrc522.Status.AllOk)
                {
                    var cardUid = uidResponse.Data;
                    _nfcReader.SelectCardUniqueId(cardUid);

                    try
                    {
                        if (_nfcReader.AuthenticateCard1A(cardUid, 7) == RFIDControllerMfrc522.Status.AllOk)
                        {
                            // Read data from sectors
                            byte blockAdress = 0;
                            var nfcResponse = _nfcReader.CardReadData(blockAdress);

                            result = System.Text.Encoding.Default.GetString(nfcResponse.Data);
                        }
                    }
                    finally
                    {
                        _nfcReader.ClearCardSelection();
                    }
                }
            }
            return result;
        }
    }
}
