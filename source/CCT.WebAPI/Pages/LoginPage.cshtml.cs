using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCT.APIService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CCT.WebAPI.Pages
{
    public class LoginPageModel : PageModel
    {
        private SettingsClient _settingCLient;
        private string _password = "cct";

        [BindProperty]
        public string LastPage { get; set; }

        [BindProperty]
        public string LabelTextWrongPassword { get; set; }

        [BindProperty]
        public string Password { get; set; }
        public async Task<ActionResult> OnGetAsync(string handler)
        {
            LastPage = handler;
            return Page();
        }

        public async Task<ActionResult> OnPostAsync(string handler)
        {
            _settingCLient = new SettingsClient();

            _password = await _settingCLient.GetPasswordAsync();

            if(Password == _password)
            {
                if(LastPage == "Index")
                {
                    return RedirectToPage("/Index", handler);
                }
                else if(LastPage == "Settings")
                {
                    return RedirectToPage("/SettingsPage", handler);
                }
            }

            LabelTextWrongPassword = "Passwort ist falsch!";

            return Page();
        }
    }
}
