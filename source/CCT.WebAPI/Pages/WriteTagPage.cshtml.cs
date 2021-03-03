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
        [BindProperty]
        public string ErrorMessage { get; set; }
        [BindProperty]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime LastTestedDate { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan LastTestedTime { get; set; }

        public WriteTagPageModel(IUnitOfWork unitOfWork)
        {
            _pc = new PersonsClient();
            _sc = new SettingsClient();
            _pipeClient = new PipeClient();
        }

        public async Task<IActionResult> OnGetAsync(string handler)
        {
            var cookieValue = Request.Cookies["MyCookieId"];
            string _pass = await _sc.GetPasswordAsync();

            if (cookieValue == null || cookieValue != _pass)
            {
                return RedirectToPage("/LoginPage", "WriteTag");
            }

            return Page();
        }

        public async Task<ActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(PhoneNumber))
            {
                ErrorMessage = "Alle Felder müssen ausgefüllt sein!";
                ModelState.AddModelError("", "Alle Felder müssen ausgefüllt sein!");
            }
            else
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

                    int isVaccinated = newPerson.IsVaccinated ? 1 : 0;

                    string nfcFormattedString = $"{FirstName};{LastName};{PhoneNumber};{isVaccinated};{lastTested};";

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
            return RedirectToPage("/WriteTagPage", "Write");
        }

        public IActionResult OnPostRead()
        {
            _pipeClient.SendMessage("SetReadMode");
            return RedirectToPage("/WriteTagPage", "Read");
        }

        public IActionResult OnPostReset()
        {
            return RedirectToPage("/WriteTagPage");
        }
    }
}
