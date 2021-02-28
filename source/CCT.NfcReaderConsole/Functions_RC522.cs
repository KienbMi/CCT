using Swan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        static IGpioPin _pulsePin;
        static RFIDControllerMfrc522 _nfcReader;

        public static void Init()
        {
            Pi.Init<BootstrapWiringPi>();

            // set IO Pin to Output
            _blinkingPin = Pi.Gpio[17];
            _blinkingPin.PinMode = GpioPinDriveMode.Output;
            _pulsePin = Pi.Gpio[18];
            _pulsePin.PinMode = GpioPinDriveMode.Output;
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

        public static void BeepSignal()
        {
            _pulsePin.Write(true);
            Thread.Sleep(300);
            _pulsePin.Write(false);
        }

        public static (bool, string) ReadTagFromRC522()
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
                    PrintUid(cardUid, uidResponse.DataBitLength);
                    _nfcReader.SelectCardUniqueId(cardUid);

                    try
                    {
                        bool authOn = false;

                        // Read data from sectors
                        byte blockAdress = 4;

                        for (int i = 0; i < 7; i++)    // 7 * 16 bytes = 112
                        {
                            if (authOn && _nfcReader.AuthenticateCard1A(RFIDControllerMfrc522.DefaultAuthKey, cardUid, (byte)(blockAdress + 3)) != RFIDControllerMfrc522.Status.AllOk)
                            {
                                Console.WriteLine("Authentication error");
                            }
                            var nfcResponse = _nfcReader.CardReadData(blockAdress);
                            result = result + Encoding.Default.GetString(nfcResponse.Data);
                            blockAdress += 4;
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
                _pulsePin.Write(false);
                Console.WriteLine("* ResetSignals *");
            }
        }

        internal static bool WriteTagRC522(string nfcNewDataContent)
        {
            if (_initDone == false)
                Init();

            string result = string.Empty;
            bool cardDetected = _nfcReader.DetectCard() == RFIDControllerMfrc522.Status.AllOk;
            bool writeDone = false;

            if (cardDetected)
            {
                var uidResponse = _nfcReader.ReadCardUniqueId();
                if (uidResponse.Status == RFIDControllerMfrc522.Status.AllOk)
                {
                    var cardUid = uidResponse.Data;
                    PrintUid(cardUid, uidResponse.DataBitLength);
                    _nfcReader.SelectCardUniqueId(cardUid);
                    int byteBlocksToWrite = 7; // 7 * 16 bytes = 112 bytes
                    int byteBlocksWritten = 0;

                    try
                    {
                        bool authOn = false;

                        // Write data to sectors
                        byte blockAdress = 4;

                        for (int i = 0; i < byteBlocksToWrite; i++)
                        {
                            if (authOn && _nfcReader.AuthenticateCard1A(RFIDControllerMfrc522.DefaultAuthKey, cardUid, (byte)(blockAdress + 3)) != RFIDControllerMfrc522.Status.AllOk)
                            {
                                Console.WriteLine("Authentication error");
                            }
                            string data = string.Concat(nfcNewDataContent.Skip(i * 16).Take(16));
                            data = (data + new string(' ', 16)).Truncate(16);
                            var writeStatus = _nfcReader.CardWriteData(blockAdress, Encoding.ASCII.GetBytes(data));

                            Console.WriteLine($"Status: {writeStatus}");
                            if (writeStatus == RFIDControllerMfrc522.Status.AllOk)
                            {                              
                                byteBlocksWritten++;
                            }

                            blockAdress += 4;
                        }

                        if (byteBlocksWritten == byteBlocksToWrite)
                        {
                            Console.WriteLine("Write done");
                            writeDone = true;
                        }
                        else
                        {
                            Console.WriteLine("Write error");
                        }
                        Console.WriteLine($"{byteBlocksWritten} of {byteBlocksToWrite} written");
                    }
                    finally
                    {
                        _nfcReader.ClearCardSelection();
                    }
                }
            }
            return writeDone;
        }

        internal static void PrintUid(byte[] cardUid, byte dataBitLength)
        {
            //Console.WriteLine($"UID Size {dataBitLength :X}");
            Console.Write("Card UID: ");
            if (cardUid == null)
            {
                Console.WriteLine("----");
                return;
            }

            foreach (var item in cardUid)
            {
                Console.Write($"{item :X} ");
            }
            Console.WriteLine();
        }
    }
}
