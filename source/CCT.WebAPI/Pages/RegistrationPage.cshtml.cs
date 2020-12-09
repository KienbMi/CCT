using System;
using System.Collections.Generic;
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
        private PersonsClient _pc;
        private readonly IUnitOfWork _uow;

        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string PhoneNumber { get; set; }

        public RegistrationPageModel(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            _pc = new PersonsClient();
        }
        public async Task OnGetAsync()
        {
        }

        public async Task<ActionResult> OnPostAsync()
        {
            try
            {
                await _pc.PostPersonAsync(new PersonDto
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    PhoneNumber = PhoneNumber,
                    RecordTime = DateTime.Now
                });
            }
            catch(ApiException<APIService.ProblemDetails> ex)
            {

            }

            return RedirectToPage("/Index");
        }
    }
}
