using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCT.Core.Contracts;
using CCT.Core.Entities;
using CCT.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CCT.Persistence.Test
{
    [TestClass]
    public class ApplicationDbContextTest
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
        public async Task ApplicationDbContext_AddPerson_SchouldPersistPerson()
        {
            string dbName = Guid.NewGuid().ToString();

            using(ApplicationDbContext dbContext = GetDbContext(dbName))
            {
                Person person = new Person { FirstName = "Hugo", LastName = "Boss", PhoneNumber = "0650/8892394" };
                Assert.AreEqual(0, person.Id);
                dbContext.Persons.Add(person);
                await dbContext.SaveChangesAsync();
                Assert.AreNotEqual(0, person.Id);
            }

            using(ApplicationDbContext verifyContext = GetDbContext(dbName))
            {
                StringBuilder logText = new StringBuilder();
                Assert.AreEqual(1, await verifyContext.Persons.CountAsync());
                Person person = await verifyContext.Persons.FirstAsync();
                Assert.IsNotNull(person);
                Assert.AreEqual("0650/8892394", person.PhoneNumber);
            }
        }

        [TestMethod]
        public async Task ApplicationDbContext_AddPersons_QueryPerson_ShouldRetrunPerson()
        {
            string dbName = Guid.NewGuid().ToString();

            using (ApplicationDbContext dbContext = GetDbContext(dbName))
            {
                Person person1 = new Person
                {
                    FirstName = "Anna",
                    LastName = "Nuss",
                    PhoneNumber = "0664/9032948"
                };
                Person person2 = new Person
                {
                    FirstName = "Hans",
                    LastName = "Gruber",
                    PhoneNumber = "0664/4255525"
                };
                Person person3 = new Person
                {
                    FirstName = "Alfred",
                    LastName = "Bauer",
                    PhoneNumber = "0650/9442548"
                };
                Person person4 = new Person
                {
                    FirstName = "Hannes",
                    LastName = "Ullisch",
                    PhoneNumber = "0680/1145636"
                };
                dbContext.Persons.Add(person1);
                dbContext.Persons.Add(person2);
                dbContext.Persons.Add(person3);
                dbContext.Persons.Add(person4);
                await dbContext.SaveChangesAsync();
                Assert.AreNotEqual(0, person1.Id);
                Assert.AreNotEqual(1, person2.Id);
                Assert.AreNotEqual(2, person3.Id);
                Assert.AreNotEqual(3, person4.Id);
            }

            using(ApplicationDbContext queryContext = GetDbContext(dbName))
            {
                Assert.AreEqual(4, await queryContext.Persons.CountAsync());

                Person firstPerson = await queryContext.Persons.OrderBy(person => person.LastName).FirstAsync();
                Assert.AreEqual("Bauer", firstPerson.LastName);
            }
        }

        [TestMethod]
        public async Task ApplicationDbContext_UpdatePerson_ShouldReturnUpdatedPerson()
        {
            string dbName = Guid.NewGuid().ToString();

            using(ApplicationDbContext dbContext = GetDbContext(dbName))
            {
                Person person = new Person
                {
                    FirstName = "Hannes",
                    LastName = "Ullisch",
                    PhoneNumber = "0680/1145636"
                };
                Assert.AreEqual(0, person.Id);
                dbContext.Persons.Add(person);
                await dbContext.SaveChangesAsync();
                Assert.AreNotEqual(0, person.Id);
            }

            using (ApplicationDbContext updateContext = GetDbContext(dbName))
            {
                Person person = await updateContext.Persons.FirstAsync();
                person.PhoneNumber = "0650/1145636";
                await updateContext.SaveChangesAsync();
            }

            using (ApplicationDbContext verifyContext = GetDbContext(dbName))
            {
                Person person = await verifyContext.Persons.FirstAsync();
                Assert.AreEqual("0650/1145636", person.PhoneNumber);
            }
        }

        [TestMethod]
        public async Task ApplicationDbContext_DeletePerson_ShouldReturnZeroPersons()
        {
            string dbName = Guid.NewGuid().ToString();

            using (ApplicationDbContext dbContext = GetDbContext(dbName))
            {
                Person person = new Person
                {
                    FirstName = "Hannes",
                    LastName = "Ullisch",
                    PhoneNumber = "0680/1145636"
                };
                Assert.AreEqual(0, person.Id);
                dbContext.Persons.Add(person);
                dbContext.Persons.Add(new Person
                {
                    FirstName = "Alfred",
                    LastName = "Bauer",
                    PhoneNumber = "0650/9442548"
                });
                await dbContext.SaveChangesAsync();
                Assert.AreNotEqual(0, person.Id);
            }

            using (ApplicationDbContext deleteContext = GetDbContext(dbName))
            {
                Person person = await deleteContext.Persons.SingleAsync(sc => sc.PhoneNumber == "0650/9442548");
                deleteContext.Persons.Remove(person);
                deleteContext.SaveChanges();
            }

            using (ApplicationDbContext verifyContext = GetDbContext(dbName))
            {
                Assert.AreEqual(1, await verifyContext.Persons.CountAsync());
                var person = await verifyContext.Persons.FirstAsync();
                Assert.AreEqual("0680/1145636", person.PhoneNumber);
            }
        }
    }

    [TestClass]
    public class UnitOfWorkTests
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
        public async Task UnitOfWork_PersonRepository_GetAllPersons_ShouldReturnAllPersons()
        {
            string dbName = Guid.NewGuid().ToString();

            using(IUnitOfWork unitOfWork = new UnitOfWork(GetDbContext(dbName)))
            {
                Person person1 = new Person
                {
                    FirstName = "Anna",
                    LastName = "Nuss",
                    PhoneNumber = "0664/9032948"
                };
                Person person2 = new Person
                {
                    FirstName = "Hans",
                    LastName = "Gruber",
                    PhoneNumber = "0664/4255525"
                };
                Person person3 = new Person
                {
                    FirstName = "Alfred",
                    LastName = "Bauer",
                    PhoneNumber = "0650/9442548"
                };
                Person person4 = new Person
                {
                    FirstName = "Hannes",
                    LastName = "Ullisch",
                    PhoneNumber = "0680/1145636"
                };

                await unitOfWork.PersonRepository.AddPersonAsync(person1);
                await unitOfWork.PersonRepository.AddPersonAsync(person2);
                await unitOfWork.PersonRepository.AddPersonAsync(person3);
                await unitOfWork.PersonRepository.AddPersonAsync(person4);
                await unitOfWork.SaveChangesAsync();
            }

            using(IUnitOfWork unitOfWOrk = new UnitOfWork(GetDbContext(dbName)))
            {
                var persons = await unitOfWOrk.PersonRepository.GetAllPersonAsync();
                Assert.AreEqual(4, persons.Count());
            }
        }

        [TestMethod]
        public async Task UnitOfWork_PersonRepository_GetPersonByPhoneNumber_ShouldReturnCorrectPerson()
        {
            string dbName = Guid.NewGuid().ToString();

            using (IUnitOfWork unitOfWork = new UnitOfWork(GetDbContext(dbName)))
            {
                Person person1 = new Person
                {
                    FirstName = "Anna",
                    LastName = "Nuss",
                    PhoneNumber = "0664/9032948"
                };
                Person person2 = new Person
                {
                    FirstName = "Hans",
                    LastName = "Gruber",
                    PhoneNumber = "0664/4255525"
                };
                Person person3 = new Person
                {
                    FirstName = "Alfred",
                    LastName = "Bauer",
                    PhoneNumber = "0650/9442548"
                };
                Person person4 = new Person
                {
                    FirstName = "Hannes",
                    LastName = "Ullisch",
                    PhoneNumber = "0680/1145636"
                };

                await unitOfWork.PersonRepository.AddPersonAsync(person1);
                await unitOfWork.PersonRepository.AddPersonAsync(person2);
                await unitOfWork.PersonRepository.AddPersonAsync(person3);
                await unitOfWork.PersonRepository.AddPersonAsync(person4);
                await unitOfWork.SaveChangesAsync();
            }

            using (IUnitOfWork unitOfWOrk = new UnitOfWork(GetDbContext(dbName)))
            {
                var person = await unitOfWOrk.PersonRepository.GetPersonByPhoneNumberAsync("0650/9442548");
                Assert.AreEqual("Alfred", person.FirstName);
                Assert.AreEqual("Bauer", person.LastName);
            }
        }

        [TestMethod]
        public async Task UnitOfWork_PersonRepository_GetPersonByDate_ShouldReturnCorrectPerson()
        {
            string dbName = Guid.NewGuid().ToString();

            using (IUnitOfWork unitOfWork = new UnitOfWork(GetDbContext(dbName)))
            {
                Person person1 = new Person
                {
                    FirstName = "Anna",
                    LastName = "Nuss",
                    PhoneNumber = "0664/9032948",
                    RecordTime = DateTime.Parse("2020-10-29")
                };
                Person person2 = new Person
                {
                    FirstName = "Hans",
                    LastName = "Gruber",
                    PhoneNumber = "0664/4255525",
                    RecordTime = DateTime.Parse("2020-10-30")
                };
                Person person3 = new Person
                {
                    FirstName = "Alfred",
                    LastName = "Bauer",
                    PhoneNumber = "0650/9442548",
                    RecordTime = DateTime.Parse("2020-11-06")
                };
                Person person4 = new Person
                {
                    FirstName = "Hannes",
                    LastName = "Ullisch",
                    PhoneNumber = "0680/1145636",
                    RecordTime = DateTime.Parse("2020-11-06")
                };

                await unitOfWork.PersonRepository.AddPersonAsync(person1);
                await unitOfWork.PersonRepository.AddPersonAsync(person2);
                await unitOfWork.PersonRepository.AddPersonAsync(person3);
                await unitOfWork.PersonRepository.AddPersonAsync(person4);
                await unitOfWork.SaveChangesAsync();
            }

            using (IUnitOfWork unitOfWOrk = new UnitOfWork(GetDbContext(dbName)))
            {
                var persons = await unitOfWOrk.PersonRepository.GetPersonsByDateAsync(DateTime.Parse("2020-11-06"));
                Assert.AreEqual("Alfred", persons.First().FirstName);
                Assert.AreEqual("Bauer", persons.First().LastName);
                Assert.AreEqual(2, persons.Count());
            }
        }

        [TestMethod]
        public async Task UnitOfWork_PersonRepository_GetPersonsForToday_ShouldReturnCorrectPerson()
        {
            string dbName = Guid.NewGuid().ToString();

            using (IUnitOfWork unitOfWork = new UnitOfWork(GetDbContext(dbName)))
            {
                Person person1 = new Person
                {
                    FirstName = "Anna",
                    LastName = "Nuss",
                    PhoneNumber = "0664/9032948",
                    RecordTime = DateTime.Now
                };
                Person person2 = new Person
                {
                    FirstName = "Hans",
                    LastName = "Gruber",
                    PhoneNumber = "0664/4255525",
                    RecordTime = DateTime.Now
                };
                Person person3 = new Person
                {
                    FirstName = "Alfred",
                    LastName = "Bauer",
                    PhoneNumber = "0650/9442548",
                    RecordTime = DateTime.Parse("2020-11-06")
                };
                Person person4 = new Person
                {
                    FirstName = "Hannes",
                    LastName = "Ullisch",
                    PhoneNumber = "0680/1145636",
                    RecordTime = DateTime.Parse("2020-11-09")
                };

                await unitOfWork.PersonRepository.AddPersonAsync(person1);
                await unitOfWork.PersonRepository.AddPersonAsync(person2);
                await unitOfWork.PersonRepository.AddPersonAsync(person3);
                await unitOfWork.PersonRepository.AddPersonAsync(person4);
                await unitOfWork.SaveChangesAsync();
            }

            using (IUnitOfWork unitOfWOrk = new UnitOfWork(GetDbContext(dbName)))
            {
                var persons = await unitOfWOrk.PersonRepository.GetPersonsForTodayAsync();
                Assert.AreEqual("Anna", persons.First().FirstName);
                Assert.AreEqual("Nuss", persons.First().LastName);
                Assert.AreEqual(2, persons.Count());
            }
        }

        [TestMethod]
        public async Task UnitOfWork_PersonRepository_GetPersonsOlderThen_ShouldReturnCorrectPerson()
        {
            string dbName = Guid.NewGuid().ToString();

            using (IUnitOfWork unitOfWork = new UnitOfWork(GetDbContext(dbName)))
            {
                Person person1 = new Person
                {
                    FirstName = "Anna",
                    LastName = "Nuss",
                    PhoneNumber = "0664/9032948",
                    RecordTime = DateTime.Now
                };
                Person person2 = new Person
                {
                    FirstName = "Hans",
                    LastName = "Gruber",
                    PhoneNumber = "0664/4255525",
                    RecordTime = DateTime.Now
                };
                Person person3 = new Person
                {
                    FirstName = "Alfred",
                    LastName = "Bauer",
                    PhoneNumber = "0650/9442548",
                    RecordTime = DateTime.Now.Subtract(TimeSpan.FromDays(30))
                };
                Person person4 = new Person
                {
                    FirstName = "Hannes",
                    LastName = "Ullisch",
                    PhoneNumber = "0680/1145636",
                    RecordTime = DateTime.Now.Subtract(TimeSpan.FromDays(31))
                };

                await unitOfWork.PersonRepository.AddPersonAsync(person1);
                await unitOfWork.PersonRepository.AddPersonAsync(person2);
                await unitOfWork.PersonRepository.AddPersonAsync(person3);
                await unitOfWork.PersonRepository.AddPersonAsync(person4);
                await unitOfWork.SaveChangesAsync();
            }


            using (IUnitOfWork unitOfWOrk = new UnitOfWork(GetDbContext(dbName)))
            {
                var persons = await unitOfWOrk.PersonRepository.GetPersonsOlderThenAsync(30);
                Assert.AreEqual("Hannes", persons.First().FirstName);
                Assert.AreEqual("Ullisch", persons.First().LastName);
                Assert.AreEqual(1, persons.Count());
            }
        }

        [TestMethod]
        public async Task UnitOfWork_PersonRepository_DeletePersonsOlderThen_ShouldReturnCorrectPerson()
        {
            string dbName = Guid.NewGuid().ToString();

            using (IUnitOfWork unitOfWork = new UnitOfWork(GetDbContext(dbName)))
            {
                Person person1 = new Person
                {
                    FirstName = "Anna",
                    LastName = "Nuss",
                    PhoneNumber = "0664/9032948",
                    RecordTime = DateTime.Now
                };
                Person person2 = new Person
                {
                    FirstName = "Hans",
                    LastName = "Gruber",
                    PhoneNumber = "0664/4255525",
                    RecordTime = DateTime.Now
                };
                Person person3 = new Person
                {
                    FirstName = "Alfred",
                    LastName = "Bauer",
                    PhoneNumber = "0650/9442548",
                    RecordTime = DateTime.Now.Subtract(TimeSpan.FromDays(31))
                };
                Person person4 = new Person
                {
                    FirstName = "Hannes",
                    LastName = "Ullisch",
                    PhoneNumber = "0680/1145636",
                    RecordTime = DateTime.Now.Subtract(TimeSpan.FromDays(31))
                };

                await unitOfWork.PersonRepository.AddPersonAsync(person1);
                await unitOfWork.PersonRepository.AddPersonAsync(person2);
                await unitOfWork.PersonRepository.AddPersonAsync(person3);
                await unitOfWork.PersonRepository.AddPersonAsync(person4);
                await unitOfWork.SaveChangesAsync();
            }

            using (IUnitOfWork unitOfWOrk = new UnitOfWork(GetDbContext(dbName)))
            {
                var personsToDelete = await unitOfWOrk.PersonRepository.GetPersonsOlderThenAsync(30);
                unitOfWOrk.PersonRepository.DeletePersons(personsToDelete);
                await unitOfWOrk.SaveChangesAsync();

                var persons = await unitOfWOrk.PersonRepository.GetAllPersonAsync();
                Assert.AreEqual("Anna", persons.First().FirstName);
                Assert.AreEqual("Nuss", persons.First().LastName);
                Assert.AreEqual(2, persons.Count());
            }
        }

        [TestMethod]
        public async Task UnitOfWork_SettingRepository_GetDefaultPasswort_ShouldReturnCorrectValue()
        {
            string dbName = Guid.NewGuid().ToString();

            using (IUnitOfWork unitOfWork = new UnitOfWork(GetDbContext(dbName)))
            {
                string password = await unitOfWork.SettingRepository.GetPasswordAsync();
                Assert.AreEqual("cct", password);
            }
        }
    }
}
