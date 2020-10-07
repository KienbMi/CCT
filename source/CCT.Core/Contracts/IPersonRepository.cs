﻿using CCT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCT.Core.Contracts
{
    public interface IPersonRepository
    {
        Task AddPersonAsync(Person person);

        Task<Person[]> GetAllPersonAsync();

        Task<Person> GetPersonByPhoneNumberAsync(string phoneNumber);
        Task<Person[]> GetPersonsByDateAsync(DateTime date);
        Task<Person[]> GetPersonsForTodayAsync();
    }
}
