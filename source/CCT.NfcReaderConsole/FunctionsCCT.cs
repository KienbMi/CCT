﻿using CCT.Core;
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
            const int IDX_VACCINATION = 3;
            const int IDX_LASTTESTED = 4;

            Person person = new Person
            {
                FirstName = data[IDX_FIRSTNAME],
                LastName = data[IDX_LASTNAME],
                PhoneNumber = data[IDX_PHONENUMBER],
                RecordTime = DateTime.Now
            };

            if (data.Length > IDX_VACCINATION + 1 && data[IDX_VACCINATION] == "1")
            {
                person.IsVaccinated = true;
            }

            if (data.Length > IDX_LASTTESTED + 1 && DateTime.TryParse(data[IDX_LASTTESTED], out var date))
            {
                person.LastTested = date;
            }

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

        public static bool AddRemoveDummyInDb(ApplicationDbContext dbContext = null)
        {
            using (UnitOfWork unitOfWork = (dbContext == null) ? new UnitOfWork() : new UnitOfWork(dbContext))
            {
                Person person = new Person { FirstName = "Dummy" };

                unitOfWork.PersonRepository.AddPerson(person);
                unitOfWork.SaveChanges();
                unitOfWork.PersonRepository.DeletePerson(person);
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

        public static async Task DeletePersonsOutsideStoragePeriode()
        {
            int storageTimeInDays = await FunctionsCCT.GetStorageDurationFromDbAsync();
            await DeletePersonsOlderThenInDbAsync(storageTimeInDays);
        }


        public static async Task<int> GetStorageDurationFromDbAsync(ApplicationDbContext dbContext = null)
        {
            using (UnitOfWork unitOfWork = (dbContext == null) ? new UnitOfWork() : new UnitOfWork(dbContext))
            {
                int storageDuration = await unitOfWork.SettingRepository.GetStorageDurationAsync();
                return storageDuration;
            }
        }

        public static async Task<NfcReaderType> GetNfcReaderTypeFromDbAsync(ApplicationDbContext dbContext = null)
        {
            using (UnitOfWork unitOfWork = (dbContext == null) ? new UnitOfWork() : new UnitOfWork(dbContext))
            {
                NfcReaderType nfcReaderType = await unitOfWork.SettingRepository.GetNfcReaderTypeAsync();
                return nfcReaderType;
            }
        }
    }

    enum Mode
    {
        Read,
        Write
    };

}
