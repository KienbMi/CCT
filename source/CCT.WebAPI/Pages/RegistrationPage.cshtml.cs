using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CCT.APIService;
using CCT.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CCT.WebAPI.Pages
{
    public class RegistrationPageModel : PageModel
    {
        private SettingsClient _sc;
        private PersonsClient _pc;
        private readonly IUnitOfWork _uow;

        [BindProperty]
        public string LabelTextAfterRegistration { get; set; }
        [BindProperty]
        public string NameAfterRegistrationText { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Vorname ist verpflichtend!")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Vorname muss mindestens 1 Zeichen lang sein!")]
        public string FirstName { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Nachname ist verpflichtend!")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Nachname muss mindestens 1 Zeichen lang sein!")]
        public string LastName { get; set; }
        [BindProperty]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Telefonnummer muss mindestens 5 Zeichen lang sein!")]
        [Required(ErrorMessage = "Telefonnummer ist verpflichtend!")]
        public string PhoneNumber { get; set; }
        [BindProperty]
        public bool IsVaccinated { get; set; }
        [BindProperty]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime LastTestedDate { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan LastTestedTime { get; set; }

        public RegistrationPageModel(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _pc = new PersonsClient();
            _sc = new SettingsClient();
            //LastTestedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //LastTestedTime = new TimeSpan(DateTime.Now.TimeOfDay.Ticks);
        }

        public async Task OnGetAsync()
        {
        }

        public async Task<ActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(PhoneNumber) && PhoneNumber[0] != '0' && PhoneNumber[0] != '+')
            {
                ModelState.AddModelError(nameof(PhoneNumber), "Geben Sie eine gültige Telefonnummer ein! Beginnend mit +43 oder 06..");
                return Page();
            }
            if (ModelState.IsValid || (ModelState.ErrorCount == 1 && ModelState[nameof(LastTestedDate)].Errors.Count > 0))
            {
                try
                {
                    var lastTested = LastTestedDate + LastTestedTime;
                    var newPerson = new PersonDto
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        PhoneNumber = PhoneNumber,
                        RecordTime = DateTime.Now,
                        IsVaccinated = IsVaccinated,
                        LastTested = lastTested
                    };

                    await _pc.PostPersonAsync(newPerson);

                    NameAfterRegistrationText = $"Hallo, {FirstName}!";
                    LabelTextAfterRegistration = await _sc.GetWelcomeTextAsync();
                }
                catch (ApiException<APIService.ProblemDetails> ex)
                {
                    ModelState.AddModelError("", ex.Result.Detail);
                }
            }
            
            return Page();
        }

        public IActionResult OnPostReset()
        {
            return RedirectToPage("RegistrationPage");
        }
    }
}
