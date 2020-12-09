using CCT.Core.Contracts;
using CCT.Core.Entities;
using CCT.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Persistence
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PersonRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPersonAsync(Person person)
            => await _dbContext.Persons.AddAsync(person);

        public void AddPerson(Person person)
            => _dbContext.Persons.Add(person);

        public void DeletePerson(Person person)
            => _dbContext.Persons.Remove(person);

        public void DeletePersons(IEnumerable<Person> persons)
            => _dbContext.Persons.RemoveRange(persons);

        public async Task<Person[]> GetAllPersonAsync()
            => await _dbContext.Persons
                .OrderBy(p => p.RecordTime)
                .ToArrayAsync();
        public async Task<Person> GetPersonByPhoneNumberAsync(string phoneNumber)
            => await _dbContext.Persons
                .Where(p => p.PhoneNumber.Equals(phoneNumber))
                .FirstOrDefaultAsync();

        public async Task<Person[]> GetPersonsByDateAsync(DateTime date)
            => await _dbContext.Persons
                .Where(p => p.RecordTime.Date == date.Date)
                .ToArrayAsync();

        public async Task<Person[]> GetPersonsForTodayAsync()
            => await _dbContext.Persons
                .Where(p => p.RecordTime.Date == DateTime.Today.Date)
                .ToArrayAsync();

        public async Task<Person[]> GetPersonsOlderThenAsync(int days)
            => await _dbContext.Persons
                .Where(p => p.RecordTime.AddDays(days + 1) <= DateTime.Now)
                .ToArrayAsync();
    }
}
