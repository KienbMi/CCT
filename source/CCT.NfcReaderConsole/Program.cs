﻿using CCT.Core.Entities;
using CCT.NfcReaderConsole;
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
            uFR.DL_STATUS dl_status;
            bool card_in_field = false;
            string nfcDataContent = string.Empty;
            char c;

            //signaling
            const byte FRES_OK_LIGHT = 0x01,    // long green
                       FERR_LIGHT = 0x02,       // long red
                       FRES_OK_SOUND = 0x01,    // short
                       FERR_SOUND = 0x00;       // none

            try
            {
                // Check database
                await FunctionsCCT.CheckDatabaseAsync();

                // Start NFC-Reader programm
                Functions.headline();
                do
                {
                    while (!Console.KeyAvailable)
                    {
                        (dl_status, nfcDataContent) = Functions.ReadLinear();

                        switch (dl_status)
                        {
                            case uFR.DL_STATUS.UFR_FT_STATUS_ERROR_2:
                            case uFR.DL_STATUS.UFR_FT_STATUS_ERROR_5:
                                Functions.reader_automaticOpen();
                                break;

                            case uFR.DL_STATUS.UFR_OK:
                                if (!card_in_field)
                                {
                                    card_in_field = true;
                                    Person person = FunctionsCCT.ParseNfcDataToPerson(nfcDataContent);

                                    if (person != null)
                                    {
                                        await FunctionsCCT.AddPersonToDbAsync(person);
                                        Console.WriteLine(nfcDataContent);
                                        uFCoder.ReaderUISignal(FRES_OK_LIGHT, FRES_OK_SOUND);
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

                        int milliseconds = 300;
                        Thread.Sleep(milliseconds);
                    }

                    c = Console.ReadKey(true).KeyChar;

                } while (c != '\x1b');
            }
            catch (System.FormatException ex)
            {
                Console.WriteLine($"Unexpected error occured:");
                Console.WriteLine($"{ex.Message}");

                Exception run = ex.InnerException;
                while (run != null)
                {
                    Console.WriteLine($"{ex.Message}");
                    run = run.InnerException;
                }
            }
            return 0;
        }
    }
}
