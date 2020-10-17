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
    public class AddPersonTest
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
    }
}
