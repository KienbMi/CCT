using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CCT.WebAPI.Pages
{
    public class SettingsPageModel : PageModel
    {
        public string LabelTextSuccess { get; set; }
        [BindProperty]
        public int StorageDuration { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string WelcomeMessage { get; set; }

        public async Task<ActionResult> OnGetAsync(string handler)
        {
            if (handler == null)
            {
                return RedirectToPage("/LoginPage", "Settings");
            }

            return Page();
        }

        public async Task<ActionResult> OnPostAsync(string handler)
        {
            //Code fürs speichern der Einstellungen in der Datenbank

            LabelTextSuccess = "Einstellungen erfolgreich gespeichert!";

            return Page();
        }
    }
}
