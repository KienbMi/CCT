using CCT.APIService;
using CCT.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCT.WebAPI.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Date { get; set; }
        [BindProperty]
        public string From { get; set; }
        [BindProperty]
        public string To { get; set; }

        private PersonsClient _pc;
        private readonly IUnitOfWork _uow;
        public ICollection<CCT.APIService.Person> PeopleOverview { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _pc = new PersonsClient();
        }

        public async Task<ActionResult> OnGetAsync(string handler)
        {
            if(handler == null)
            {
                return RedirectToPage("/LoginPage", "Index");
            }

            ViewData["Message"] = "Personal Data";

            //await PrepareDb();
            PeopleOverview = await _pc.GetAllPersonsAsync();

            return Page();
        }

        public async Task<ActionResult> OnPostAsync()
        {
            bool isValid = true;
            DateTime date = DateTime.Today;
            TimeSpan timeFrom = TimeSpan.Zero;
            TimeSpan timeTo = TimeSpan.FromDays(1).Subtract(TimeSpan.FromSeconds(1));

            if (!DateTime.TryParse(Date, out date))
            {
                isValid = false;
                ModelState.AddModelError(nameof(Date), "Bitte gültiges Datumformat eingeben (dd.MM.yyyy)");
            }

            if (!String.IsNullOrWhiteSpace(From) && !TimeSpan.TryParse(From, out timeFrom))
            {
                isValid = false;
                ModelState.AddModelError(nameof(From), "Bitte gültiges Zeitformat eingeben (hh:mm)");
            }

            if (!String.IsNullOrWhiteSpace(To) && !TimeSpan.TryParse(To, out timeTo))
            {
                isValid = false;
                ModelState.AddModelError(nameof(To), "Bitte gültiges Zeitformat eingeben (hh:mm)");
            }

            if (!isValid)
            {
                PeopleOverview = await _pc.GetAllPersonsAsync();
                return Page();
            }

            DateTime from = date + timeFrom;
            DateTime to = date + timeTo;

            PeopleOverview = await _pc.GetPersonsForTimespanAsync(from, to);

            return Page();
        }

        private async Task PrepareDb()
        {
            await _uow.PersonRepository.AddPersonAsync(new CCT.Core.Entities.Person { FirstName = "Hannes", LastName = "Berger", PhoneNumber = "0650/8893128", RecordTime = DateTime.Now });
            await _uow.PersonRepository.AddPersonAsync(new CCT.Core.Entities.Person { FirstName = "Peter", LastName = "Auinger", PhoneNumber = "0650/1244418", RecordTime = DateTime.Now });
            await _uow.PersonRepository.AddPersonAsync(new CCT.Core.Entities.Person { FirstName = "Anna", LastName = "Berger", PhoneNumber = "0664/881141283", RecordTime = DateTime.Now });
            await _uow.SaveChangesAsync();
        }
    }
}
