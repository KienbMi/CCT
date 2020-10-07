using CCT.Core.Contracts;
using CCT.Core.Entities;
using ClubAdministration.Persistence;
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

        public async Task<Person[]> GetAllPersonAsync()
            => await _dbContext.Persons
                .OrderBy(p => p.RecordTime)
                .ToArrayAsync();
    }
}
