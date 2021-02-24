using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CCT.APIService;
using CCT.Core.Contracts;
using NamedPipe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CCT.WebAPI.Pages
{
    public class WriteTagPageModel : PageModel
    {
        private SettingsClient _sc;
        private PersonsClient _pc;
        private PipeClient _pipeClient;

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

        public WriteTagPageModel(IUnitOfWork unitOfWork)
        {
            _pc = new PersonsClient();
            _sc = new SettingsClient();
            _pipeClient = new PipeClient();
        }

        public async Task OnGetAsync()
        {
        }

        public async Task<ActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
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

                    int isVaccinated = newPerson.IsVaccinated ? 1 : 0;

                    string nfcFormattedString = $"{FirstName};{LastName};{PhoneNumber};{newPerson.RecordTime};{isVaccinated};";

                    _pipeClient.SendMessage(nfcFormattedString);

                }
                catch (ApiException<APIService.ProblemDetails> ex)
                {
                    ModelState.AddModelError("", ex.Result.Detail);
                }
            }

            return Page();
        }

        public IActionResult OnPostWrite()
        {
            _pipeClient.SendMessage("SetWriteMode");
            return Page();
        }

        public IActionResult OnPostRead()
        {
            _pipeClient.SendMessage("SetReadMode");
            return Page();
        }

        public IActionResult OnPostReset()
        {
            return RedirectToPage("RegistrationPage");
        }
    }
}
