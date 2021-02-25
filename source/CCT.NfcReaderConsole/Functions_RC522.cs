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

        public static (bool,string) ReadTagFromRC522()
        {
            if (_initDone == false)
                Init();

            string result = string.Empty;
            bool cardDetected = _nfcReader.DetectCard() == RFIDControllerMfrc522.Status.AllOk;

            if (cardDetected)
            {
                var uidResponse = _nfcReader.ReadCardUniqueId();
                if (uidResponse.Status == RFIDControllerMfrc522.Status.AllOk)
                {
                    var cardUid = uidResponse.Data;
                    _nfcReader.SelectCardUniqueId(cardUid);

                    try
                    {
                        if (true || _nfcReader.AuthenticateCard1A(cardUid, 7) == RFIDControllerMfrc522.Status.AllOk)
                        {
                            // Read data from sectors
                            byte blockAdress = 4;

                            for (int i = 0; i < 7; i++)    // 7 * 16 bytes = 112
                            {
                                var nfcResponse = _nfcReader.CardReadData(blockAdress);
                                result = result + System.Text.Encoding.Default.GetString(nfcResponse.Data);
                                blockAdress += 4;
                            }                            
                        }
                        else
                        {
                            Console.WriteLine("Authentication error");
                        }
                    }
                    finally
                    {
                        _nfcReader.ClearCardSelection();
                    }
                }
            }
            return (cardDetected, result);
        }

        public static void ResetSignals()
        {
            if (_initDone)
            {
                _blinkingPin.Write(false);
                Console.WriteLine("* ResetSignals *");
            }
        }
    }
}
