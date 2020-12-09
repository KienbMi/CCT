using CCT.Core.Entities;
using CCT.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCT.NfcReaderConsole
{
    public class FunctionsCCT
    {
        public static Person ParseNfcDataToPerson(string nfcData)
        {
            if (string.IsNullOrEmpty(nfcData))
            {
                Console.WriteLine("NFC data content is null or empty");
                return null;
            }

            string[] data = nfcData.Split(";");
            int dataParts = 3;

            if (data == null || data.Length < (dataParts + 1))
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

        public static bool AddPersonToDb(Person person, ApplicationDbContext dbContext = null)
        {
            using (UnitOfWork unitOfWork = (dbContext == null) ? new UnitOfWork() : new UnitOfWork(dbContext))
            {
                unitOfWork.PersonRepository.AddPerson(person);
                unitOfWork.SaveChanges();
            }
            return true;
        }

        public static async Task DeletePersonsOlderThenInDbAsync(int days, ApplicationDbContext dbContext = null)
        {
            using (UnitOfWork unitOfWork = (dbContext == null) ? new UnitOfWork() : new UnitOfWork(dbContext))
            {
                var personsToDelete = await unitOfWork.PersonRepository.GetPersonsOlderThenAsync(days);
                unitOfWork.PersonRepository.DeletePersons(personsToDelete);
                unitOfWork.SaveChanges();
            }
        }

        public static void CheckDatabase(ApplicationDbContext dbContext = null)
        {
            Console.WriteLine("Datenbank Test wird gestartet");

            using (UnitOfWork unitOfWork = (dbContext == null) ? new UnitOfWork() : new UnitOfWork(dbContext))
            {
                if (unitOfWork.Exists())
                {
                    Console.WriteLine("Datenbankprüfung abgeschlossen");
                    return;
                }
                else
                {
                    Console.WriteLine("Datenbank migrieren");
                    unitOfWork.MigrateDatabase();
                }
            }
        }
    }
}
