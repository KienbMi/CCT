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
    public class PersonsModel : PageModel
    {
        private PersonsClient _personClient;
        public Person[] PersonsOverview { get; set; }

        public PersonsModel()
        {
            
        }

        public async Task OnGetAsync()
        {
            PersonsOverview = await _personClient.GetAllPersonsAsync();
        }
    }
}