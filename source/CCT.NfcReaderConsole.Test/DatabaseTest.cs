using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCT.Core.Contracts;
using CCT.Core.Entities;
using CCT.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CCT.NfcReaderConsole.Test
{
    [TestClass]
    public class DatabaseTest
    {
        private ApplicationDbContext GetDbContext(string dbName)
        {
            // Build the ApplicationDbContext 
            //  - with InMemory-DB
            return new ApplicationDbContext(
              new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .EnableSensitiveDataLogging()
                .Options);
        }

        [TestMethod]
        public async Task D01_FirstDataAccessTest()
        {
            string dbName = Guid.NewGuid().ToString();

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

            using (ApplicationDbContext dbContext = GetDbContext(dbName))
            {
                foreach (var person in persons)
                {
                    dbContext.Persons.Add(person);
                }
                await dbContext.SaveChangesAsync();
            }
            using (ApplicationDbContext dbContext = GetDbContext(dbName))
            {
                var firstOrDefault = await dbContext.Persons.FirstOrDefaultAsync<Person>();
                Assert.IsNotNull(firstOrDefault);
                Assert.AreEqual(persons[0].LastName, firstOrDefault.LastName);

                var lastOrDefault = await dbContext.Persons.LastOrDefaultAsync<Person>();
                Assert.IsNotNull(lastOrDefault);
                Assert.AreEqual(persons[1].LastName, lastOrDefault.LastName);
            }
        }

        [TestMethod]
        public async Task D02_FirstDataAccessWithUow()
        {
            string dbName = Guid.NewGuid().ToString();
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

            using (IUnitOfWork unitOfWork = new UnitOfWork(GetDbContext(dbName)))
            {
                foreach (Person person in persons)
                {
                    await unitOfWork.PersonRepository.AddPersonAsync(person);
                }
                await unitOfWork.SaveChangesAsync();
            }
            using (IUnitOfWork unitOfWork = new UnitOfWork(GetDbContext(dbName)))
            {
                Person[] personsInDb = await unitOfWork.PersonRepository.GetAllPersonAsync();
                Assert.IsNotNull(personsInDb);
                Assert.AreEqual(2, personsInDb.Length);
            }
        }

        [TestMethod]
        public void D03_DatabaseExistsTest()
        {
            //Arrange
            string dbName = Guid.NewGuid().ToString();
            bool dbExists;

            //Act
            using (IUnitOfWork unitOfWork = new UnitOfWork(GetDbContext(dbName)))
            {
                dbExists = unitOfWork.Exists();
            }

            //Assert
            Assert.IsTrue(dbExists);
        }

        [TestMethod]
        public async Task D04_AddPersonTest()
        {
            //Arrange
            string dbName = Guid.NewGuid().ToString();
            string expectedFirstName = "Max";
            string expectedLastName = "Mustermann";
            string expectedPhoneNumber = "066412345667";
            Person person = new Person
            {
                FirstName = expectedFirstName,
                LastName = expectedLastName,
                PhoneNumber = expectedPhoneNumber,
                RecordTime = DateTime.Now
            };

            //Act
            await FunctionsCCT.AddPersonToDbAsync(person, GetDbContext(dbName));

            //Assert
            using (IUnitOfWork unitOfWork = new UnitOfWork(GetDbContext(dbName)))
            {
                Person[] personsInDb = await unitOfWork.PersonRepository.GetAllPersonAsync();
                Assert.IsNotNull(personsInDb);
                Assert.AreEqual(1, personsInDb.Length);
                Assert.AreEqual(expectedFirstName, person.FirstName);
                Assert.AreEqual(expectedLastName, person.LastName);
                Assert.AreEqual(expectedPhoneNumber, person.PhoneNumber);
                Assert.AreEqual(DateTime.Now.Date, person.RecordTime.Date);
                Assert.AreEqual(DateTime.Now.Hour, person.RecordTime.Hour);
            }
        }
    }
}
