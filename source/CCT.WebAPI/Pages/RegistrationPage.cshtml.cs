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

        public RegistrationPageModel(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _pc = new PersonsClient();
            _sc = new SettingsClient();
        }

        public async Task OnGetAsync()
        {
        }

        public async Task<ActionResult> OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var newPerson = new PersonDto
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        PhoneNumber = PhoneNumber,
                        RecordTime = DateTime.Now,
                        IsVaccinated = IsVaccinated
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
