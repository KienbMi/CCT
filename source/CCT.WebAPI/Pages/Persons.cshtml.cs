using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCT.Core.Contracts;
using CCT.Core.Entities;
using CCT.Persistence;
using CCT.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CCT.WebAPI.Pages
{
    public class PersonsModel : PageModel
    {
        private readonly IUnitOfWork _uow;
        public Person[] PersonsOverview { get; set; }

        public PersonsModel(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task OnGetAsync()
        {
            await PrepareDb();
            PersonsOverview = await _uow.PersonRepository.GetAllPersonAsync();
        }

        private async Task PrepareDb()
        {
            await _uow.DeleteDatabaseAsync();

            await _uow.PersonRepository.AddPersonAsync(new Person { FirstName = "Hannes", LastName = "Berger", PhoneNumber = "0650/8893128", RecordTime = DateTime.Now });
            await _uow.PersonRepository.AddPersonAsync(new Person { FirstName = "Peter", LastName = "Auinger", PhoneNumber = "0650/1244418", RecordTime = DateTime.Now });
            await _uow.PersonRepository.AddPersonAsync(new Person { FirstName = "Anna", LastName = "Berger", PhoneNumber = "0664/881141283", RecordTime = DateTime.Now });
            await _uow.SaveChangesAsync();
        }
    }
}