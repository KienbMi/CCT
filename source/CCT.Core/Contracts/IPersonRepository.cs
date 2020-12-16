using CCT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Core.Contracts
{
    public interface IPersonRepository
    {
        Task AddPersonAsync(Person person);
        void AddPerson(Person person);

        Task<Person[]> GetAllPersonAsync();
        Task<Person[]> GetPersonsForTimeSpanAsync(DateTime from, DateTime to);
        Task<Person> GetPersonByPhoneNumberAsync(string phoneNumber);
        Task<Person[]> GetPersonsByDateAsync(DateTime date);
        Task<Person[]> GetPersonsForTodayAsync();
    }
}
