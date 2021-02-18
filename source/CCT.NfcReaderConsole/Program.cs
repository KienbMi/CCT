using CCT.Core;
using CCT.Core.Entities;
using CCT.NfcReaderConsole;
using NamedPipe;
using System;
using System.Threading;
using System.Threading.Tasks;
using uFR;

namespace ufr_mfp_console
{
    class Program
    {      
        static async Task<int> Main(string[] args)
        {
            string _actEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            bool _card_in_field_uFR = false, _card_in_field_RC522 = false;
            DateTime _dateOld = DateTime.Now;
            int cyleTime = 300; // milliseconds
            char c;

            Mode _operationMode = Mode.Read;
            NfcReaderType _nfcReaderType = NfcReaderType.uFr;

            try
            {
                // Check database
                FunctionsCCT.CheckDatabase();
                FunctionsCCT.AddRemoveDummyInDb();

                // Delete persons older then storage time (default 30 days) from database
                await FunctionsCCT.DeletePersonsOutsideStoragePeriode();

                // Get NfcReaderType from database
                _nfcReaderType = await FunctionsCCT.GetNfcReaderTypeFromDbAsync();

                // Start NFC-Reader programm
                Functions_uFR.headline();

                // Start PipeServer
                var pipeServer = new PipeServer(autoRun: true);

                // Init Reader
                switch (_nfcReaderType)
                {
                    case NfcReaderType.uFr:            
                        Functions_uFR.reader_automaticOpen();
                        break;
                    case NfcReaderType.RC522:
                        if (_actEnvironment != null && _actEnvironment.StartsWith("RPI"))
                        {
                            // toDo
                        }
                        break;
                    default:
                        Console.WriteLine("Kein gültiger NFC-Readertyp angewählt");
                        break;
                }
                
                do
                {
                    while ((!Console.IsInputRedirected)? !Console.KeyAvailable : true)
                    {
                        // Get message from Web - Application
                        string message = pipeServer.ReceiceMessage();
                        string nfcNewDataContent = string.Empty;

                        switch (message)
                        {
                            case "ModeRead":
                                _operationMode = Mode.Read;
                                break;
                            case "ModeWrite":
                                _operationMode = Mode.Write;
                                break;
                            default:
                                nfcNewDataContent = message;
                                break;
                        }

                        //nfcNewDataContent = "Max;Mustermann;07742311;1;"; // toDelete

                        if (_operationMode == Mode.Read)
                        {
                            switch (_nfcReaderType)
                            {
                                case NfcReaderType.uFr:
                                    ReadCycle_uFR(ref _card_in_field_uFR);
                                    break;
                                case NfcReaderType.RC522:
                                    ReadCycle_RC522(_actEnvironment, ref _card_in_field_RC522);
                                    break;
                                default:
                                    Console.WriteLine("Kein gültiger NFC-Readertyp angewählt");
                                    break;
                            }
                        }
                        else if (_operationMode == Mode.Write)
                        {
                            switch (_nfcReaderType)
                            {
                                case NfcReaderType.uFr:
                                    WriteCycle_uFR(nfcNewDataContent);
                                    break;
                                case NfcReaderType.RC522:
                                    WriteCycle_RC522(nfcNewDataContent);
                                    break;
                                default:
                                    Console.WriteLine("Kein gültiger NFC-Readertyp angewählt");
                                    break;
                            }
                        }
                      
                        Thread.Sleep(cyleTime);

                        // Delete persons older then storage time (default 30 days) from database
                        if (DateTime.Now.Date != _dateOld.Date)
                        {
                            await FunctionsCCT.DeletePersonsOutsideStoragePeriode();
                        }
                        _dateOld = DateTime.Now;
                    }

                    c = Console.ReadKey(true).KeyChar;

                } while (c != '\x1b');
            }
            catch (System.FormatException ex)
            {
                Console.WriteLine($"Unexpected error occured:");
                WriteExceptions(ex);
            }

            Functions_RC522.ResetSignals();
            return 0;
        }

        static void WriteExceptions(Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            Exception run = ex.InnerException;
            while (run != null)
            {
                Console.WriteLine($"{run.Message}");
                run = run.InnerException;
            }
        }

        /// <summary>
        /// Read Nfc Tag from µFR Reader and transfer data to database
        /// </summary>
        static void ReadCycle_uFR(ref bool card_in_field)
        {
            //signaling
            const byte FRES_OK_LIGHT = 0x01,    // long green
                       FERR_LIGHT = 0x02,       // long red
                       FRES_OK_SOUND = 0x01,    // short
                       FERR_SOUND = 0x00;       // none

            uFR.DL_STATUS dl_status;
            string nfcDataContent = string.Empty;

            (dl_status, nfcDataContent) = Functions_uFR.ReadLinear();

            switch (dl_status)
            {
                case uFR.DL_STATUS.UFR_FT_STATUS_ERROR_2:
                case uFR.DL_STATUS.UFR_FT_STATUS_ERROR_5:
                    Functions_uFR.reader_automaticOpen();
                    break;

                case uFR.DL_STATUS.UFR_OK:
                    if (!card_in_field)
                    {
                        card_in_field = true;
                        Person person = FunctionsCCT.ParseNfcDataToPerson(nfcDataContent);

                        if (person != null)
                        {
                            bool dbSaveOk = false;
                            try
                            {
                                dbSaveOk = FunctionsCCT.AddPersonToDb(person);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Could not write to database");
                                uFCoder.ReaderUISignal(FERR_LIGHT, FERR_SOUND);
                                WriteExceptions(ex);
                            }
                            Console.WriteLine(nfcDataContent);
                            if (dbSaveOk)
                            {
                                uFCoder.ReaderUISignal(FRES_OK_LIGHT, FRES_OK_SOUND);
                            }
                        }
                        else
                        {
                            uFCoder.ReaderUISignal(FERR_LIGHT, FERR_SOUND);
                        }
                    }
                    break;

                case uFR.DL_STATUS.UFR_NO_CARD:
                    card_in_field = false;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Write data into Nfc Tag
        /// </summary>
        static void WriteCycle_uFR(string nfcNewDataContent)
        {
            //signaling
            const byte FRES_OK_LIGHT = 0x01,    // long green
                       FERR_LIGHT = 0x02,       // long red
                       FRES_OK_SOUND = 0x01,    // short
                       FERR_SOUND = 0x00;       // none

            uFR.DL_STATUS dl_status;

            bool signalingOn = false;
            if (!string.IsNullOrEmpty(nfcNewDataContent))
            {
                (dl_status) = Functions_uFR.WriteLinear(nfcNewDataContent);
                signalingOn = true;
            }
            else
            {
                (dl_status, _) = Functions_uFR.ReadLinear();
            }

            switch (dl_status)
            {
                case uFR.DL_STATUS.UFR_FT_STATUS_ERROR_2:
                case uFR.DL_STATUS.UFR_FT_STATUS_ERROR_5:
                    Functions_uFR.reader_automaticOpen();
                    break;

                case uFR.DL_STATUS.UFR_OK:
                    if (signalingOn)
                    {
                        uFCoder.ReaderUISignal(FRES_OK_LIGHT, FRES_OK_SOUND);
                    }
                    break;

                default:
                    if (signalingOn)
                    {
                        uFCoder.ReaderUISignal(FERR_LIGHT, FERR_SOUND);
                    }
                    break;
            }
        }

        static void ReadCycle_RC522(string actEnvironment, ref bool card_in_field)
        {
            if (actEnvironment == null || actEnvironment.StartsWith("RPI") == false)
                return;

            Functions_RC522.InvertLedSignal();

            string nfcDataContent = Functions_RC522.ReadTagFromRC522();

            // data from reader received ?
            if (string.IsNullOrEmpty(nfcDataContent) == false && card_in_field == false)
            {
                card_in_field = true;
                Console.WriteLine(nfcDataContent);
                Person person = FunctionsCCT.ParseNfcDataToPerson(nfcDataContent);

                if (person != null)
                {
                    bool dbSaveOk = false;
                    try
                    {
                        dbSaveOk = FunctionsCCT.AddPersonToDb(person);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Could not write to database");
                        // Signal not ok ??;
                        WriteExceptions(ex);
                    }
                    Console.WriteLine(nfcDataContent);
                    if (dbSaveOk)
                    {
                        // Signal ok;
                    }
                }
            }
            else
            {
                card_in_field = false;
            }

            Thread.Sleep(500);
        }

        private static void WriteCycle_RC522(string nfcNewDataContent)
        {
            throw new NotImplementedException();
        }
    }
}
