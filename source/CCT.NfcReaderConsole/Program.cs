using CCT.Core.Entities;
using CCT.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
            byte sak = 0, uid_size = 0, old_sak = 0, old_uid_size = 0;
            byte[] uid = new byte[10];
            byte[] old_uid = new byte[10];
            char c

            //await StartDatabaseTestAsync();

            // ToDo Autostartup 

            Functions.reader_open();
            Functions.usage();

            do
            {
                while (!Console.KeyAvailable)
                {
                    dl_status = uFCoder.GetCardIdEx(out sak, uid, out uid_size);

                    switch (dl_status)
                    {
                        case uFR.DL_STATUS.UFR_OK:

                            if (card_in_field)
                            {
                                if (old_sak != sak || old_uid_size != uid_size || (Enumerable.SequenceEqual(uid, old_uid) == false))
                                {
                                    old_sak = sak;
                                    old_uid_size = uid_size;
                                    Array.Copy(uid, old_uid, uid.Length);
                                    //Functions.New_card_in_field(sak, ref uid, uid_size);
                                }
                            }
                            else
                            {
                                old_sak = sak;
                                old_uid_size = uid_size;
                                Array.Copy(uid, old_uid, uid.Length);
                                //Functions.New_card_in_field(sak, ref uid, uid_size);
                                await StartLinearReadAsync();
                                card_in_field = true;
                            }

                            break;

                        case uFR.DL_STATUS.UFR_NO_CARD:
                            card_in_field = false;
                            dl_status = uFR.DL_STATUS.UFR_OK;
                            break;

                        default:
                            break;
                    }
                }

                c = Console.ReadKey(true).KeyChar;

                if (c != '\x1b' && (byte)c != 0)
                {
                    Functions.menu(c);
                }
                else if ((byte)c == 0)
                {

                }
                else if (c == '\x1b')
                {
                    break;
                }
            } while (c != '\x1b');

            return 0;
        }

        private static async Task StartDatabaseTestAsync()
        {
            Console.WriteLine("Datenbank Test wird gestartet");

            using UnitOfWork unitOfWork = new UnitOfWork();
            Console.WriteLine("Datenbank löschen");
            await unitOfWork.DeleteDatabaseAsync();
            Console.WriteLine("Datenbank migrieren");
            await unitOfWork.MigrateDatabaseAsync();

            List<Person> persons = new List<Person>
            {   new Person 
                {
                    FirstName = "Max",
                    LastName = "Mustermann",
                    PhoneNumber = "066412345576",
                    RecordTime = DateTime.Now
                },
                new Person
                {
                    FirstName = "Marta",
                    LastName = "Musterfrau",
                    PhoneNumber = "066433789997",
                    RecordTime = DateTime.Now
                }
            };

            foreach (var person in persons)
            {
                await unitOfWork.PersonRepository.AddPersonAsync(person);
            }

            int savedRows = await unitOfWork.SaveChangesAsync();

            Console.WriteLine($"{savedRows} Datensätze wurden in Datenbank gespeichert!");
            Console.WriteLine();
            Console.Write("Beenden mit Eingabetaste ...");
            Console.ReadLine();
        }

        private static async Task StartLinearReadAsync()
        {
            const int IDX_FIRSTNAME = 0;
            const int IDX_LASTNAME = 1;
            const int IDX_PHONENUMBER = 2;

            //authenticate
            const byte MIFARE_AUTHENT1A = 0x60;

            //signaling
            const byte FRES_OK_LIGHT = 0x01,    // long green
                       FERR_LIGHT = 0x02,       // long red
                       FRES_OK_SOUND = 0x01,    // short
                       FERR_SOUND = 0x00;       // none

            try
            {
                ushort ushLinearAddress = 0;
                ushort ushDataLength = 100;
                byte[] baReadData = new byte[ushDataLength];
                ushort ushBytesRet = 0;
                byte bAuthMode = MIFARE_AUTHENT1A;
                byte bKeyIndex = 0;
                DL_STATUS iFResult;

                iFResult = uFCoder.LinearRead(baReadData, ushLinearAddress, ushDataLength, out ushBytesRet, bAuthMode, bKeyIndex);               

                if (iFResult == uFR.DL_STATUS.UFR_OK)
                {
                    string nfcInput = System.Text.Encoding.Default.GetString(baReadData);
                    string[] data = nfcInput.Split(";");

                    if (data != null && data.Length >= 2)
                    {
                        Console.WriteLine(nfcInput);
                        uFCoder.ReaderUISignal(FRES_OK_LIGHT, FRES_OK_SOUND);

                        Person person = new Person
                        {
                            FirstName = data[IDX_FIRSTNAME],
                            LastName = data[IDX_LASTNAME],
                            PhoneNumber = data[IDX_PHONENUMBER],
                            RecordTime = DateTime.Now
                        };

                        using (UnitOfWork unitOfWork = new UnitOfWork())
                        {
                            await unitOfWork.PersonRepository.AddPersonAsync(person);
                            int savedRows = await unitOfWork.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        uFCoder.ReaderUISignal(FERR_LIGHT, FERR_SOUND);
                    }
                }
                else
                {
                    uFCoder.ReaderUISignal(FERR_LIGHT, FERR_SOUND);
                }

            }
            catch (System.FormatException ex)
            {
                Console.WriteLine($"Error:");
                Console.WriteLine($"{ex.Message}");
                
                Exception run = ex.InnerException;
                while (run != null)
                {
                    Console.WriteLine($"{ex.Message}");
                    run = run.InnerException;
                }
            }
        }
    }
}
