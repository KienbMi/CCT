using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCT.APIService;
using CCT.Core.Contracts;
using CCT.WebAPI.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CCT.WebAPI.Pages
{
	public class RegistrationOverviewModel : PageModel
	{
		[BindProperty]
		public string Date { get; set; }
		[BindProperty]
		public string From { get; set; }
		[BindProperty]
		public string To { get; set; }

		private PersonsClient _pc;
		private SettingsClient _settingsClient;
		private readonly IUnitOfWork _uow;
		public ICollection<CCT.APIService.Person> PeopleOverview { get; set; }

		public RegistrationOverviewModel(IUnitOfWork unitOfWork)
		{
			_uow = unitOfWork;
			_pc = new PersonsClient();
			_settingsClient = new SettingsClient();
		}

		public async Task<ActionResult> OnGetAsync(string handler)
		{
			// ------- request cookie ---------------------
			var cookieValue = Request.Cookies["MyCookieId"];
			string _pass = await _settingsClient.GetPasswordAsync();
			// --------------------------------------------
			if (handler == null || cookieValue == null || cookieValue != _pass)
			{
				return RedirectToPage("/LoginPage", "Index");
			}

			ViewData["Message"] = "Personal Data";

			//await PrepareDb();
			PeopleOverview = await _pc.GetAllPersonsAsync();

			foreach (var person in PeopleOverview)
			{
				person.PhoneNumber = PhoneNumberFormatter.FormatPhoneNumber(person.PhoneNumber);
			}

			return Page();
		}

		public async Task<ActionResult> OnPostFilterAsync()
		{
			// ------- request cookie ---------------------
			var cookieValue = Request.Cookies["MyCookieId"];
			string _pass = await _settingsClient.GetPasswordAsync();

			if (cookieValue == null || cookieValue != _pass)
			{
				return RedirectToPage("/LoginPage", "Index");
			}
			// --------------------------------------------
			bool isValid = true;
			DateTime date = DateTime.Today;
			TimeSpan timeFrom = TimeSpan.Zero;
			TimeSpan timeTo = TimeSpan.FromDays(1).Subtract(TimeSpan.FromSeconds(1));

			if (!DateTime.TryParse(Date, out date))
			{
				isValid = false;
				ModelState.AddModelError(nameof(Date), "Bitte g�ltiges Datumformat eingeben (dd.MM.yyyy)");
			}

			if (!String.IsNullOrWhiteSpace(From) && !TimeSpan.TryParse(From, out timeFrom))
			{
				isValid = false;
				ModelState.AddModelError(nameof(From), "Bitte g�ltiges Zeitformat eingeben (hh:mm)");
			}

			if (!String.IsNullOrWhiteSpace(To) && !TimeSpan.TryParse(To, out timeTo))
			{
				isValid = false;
				ModelState.AddModelError(nameof(To), "Bitte g�ltiges Zeitformat eingeben (hh:mm)");
			}

			if (!isValid)
			{
				PeopleOverview = await _pc.GetAllPersonsAsync();
				return Page();
			}

			DateTime from = date + timeFrom;
			DateTime to = date + timeTo;

			PeopleOverview = await _pc.GetPersonsForTimespanAsync(from, to);

            foreach (var person in PeopleOverview)
            {
				person.PhoneNumber = PhoneNumberFormatter.FormatPhoneNumber(person.PhoneNumber);
            }

			return Page();
		}

		public async Task<ActionResult> OnPostDeleteAsync()
		{
			// ------- request cookie ---------------------
			var cookieValue = Request.Cookies["MyCookieId"];
			string _pass = await _settingsClient.GetPasswordAsync();

			if (cookieValue == null || cookieValue != _pass)
			{
				return RedirectToPage("/LoginPage", "Index");
			}
			// --------------------------------------------

			PeopleOverview = await _pc.GetAllPersonsAsync();

			return Page();
		}

		private async Task PrepareDb()
		{
			await _uow.PersonRepository.AddPersonAsync(new CCT.Core.Entities.Person { FirstName = "Hannes", LastName = "Berger", PhoneNumber = "06508893128", RecordTime = DateTime.Now });
			await _uow.PersonRepository.AddPersonAsync(new CCT.Core.Entities.Person { FirstName = "Peter", LastName = "Auinger", PhoneNumber = "06501244418", RecordTime = DateTime.Now });
			await _uow.PersonRepository.AddPersonAsync(new CCT.Core.Entities.Person { FirstName = "Anna", LastName = "Berger", PhoneNumber = "0664881141283", RecordTime = DateTime.Now });
			await _uow.SaveChangesAsync();
		}
	}
}
