using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCT.APIService;
using CCT.Core.Contracts;
using CCT.Core.Entities;
using CCT.Persistence;
using CCT.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CCT.WebAPI.Pages
{
    public class PeopleModel : PageModel
    {
        private PersonsClient _pc;
        private readonly IUnitOfWork _uow; 
        public ICollection<APIService.Person> PeopleOverview { get; set; }

        public PeopleModel(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _pc = new PersonsClient();
        }

        public async Task OnGetAsync()
        {
            ViewData["Message"] = "Personal Data";

            //await PrepareDb();
            PeopleOverview = await _pc.GetAllPersonsAsync();
        }

        private async Task PrepareDb()
        {
            await _uow.PersonRepository.AddPersonAsync(new Core.Entities.Person { FirstName = "Hannes", LastName = "Berger", PhoneNumber = "0650/8893128", RecordTime = DateTime.Now });
            await _uow.PersonRepository.AddPersonAsync(new Core.Entities.Person { FirstName = "Peter", LastName = "Auinger", PhoneNumber = "0650/1244418", RecordTime = DateTime.Now });
            await _uow.PersonRepository.AddPersonAsync(new Core.Entities.Person { FirstName = "Anna", LastName = "Berger", PhoneNumber = "0664/881141283", RecordTime = DateTime.Now });
            await _uow.SaveChangesAsync();
        }
    }
}