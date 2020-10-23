using CCT.Core.Entities;
using CCT.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using uFR;

namespace ufr_mfp_examples_c_sharp_console
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

            // Check database
            await CheckDatabaseAsync();

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
                                Person person = ParseNfcDataToPerson(nfcDataContent);

                                if (person != null)
                                {
                                    await AddPersonToDbAsync(person);
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

            return 0;
        }

        #region helpers
        private static Person ParseNfcDataToPerson(string nfcData)
        {
            if (string.IsNullOrEmpty(nfcData))
            {
                Console.WriteLine("NFC data content is null or empty");
                return null;
            }

            string[] data = nfcData.Split(";");
            int dataParts = 3;
            
            if (data == null || data.Length < dataParts)
            {
                Console.WriteLine($"NFC data content is not valid => raw data '{nfcData}'");
                return null;
            }

            const int IDX_FIRSTNAME = 0;
            const int IDX_LASTNAME = 1;
            const int IDX_PHONENUMBER = 2;

            Person person = new Person
            {
                FirstName = data[IDX_FIRSTNAME],
                LastName = data[IDX_LASTNAME],
                PhoneNumber = data[IDX_PHONENUMBER],
                RecordTime = DateTime.Now
            };

            return person;
        }

        private static async Task AddPersonToDbAsync(Person person)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                await unitOfWork.PersonRepository.AddPersonAsync(person);
                await unitOfWork.SaveChangesAsync();
            }
        }

        private static async Task CheckDatabaseAsync()
        {
            Console.WriteLine("Datenbank Test wird gestartet");

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.Exists())
                {
                    Console.WriteLine("Datenbankprüfung abgeschlossen");
                    return;
                }
                else
                {
                    Console.WriteLine("Datenbank migrieren");
                    await unitOfWork.MigrateDatabaseAsync();
                }
            }
        }
        #endregion
    }
}
