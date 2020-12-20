using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCT.APIService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProblemDetails = CCT.APIService.ProblemDetails;

namespace CCT.WebAPI.Pages
{
    public class SettingsPageModel : PageModel
    {
        private SettingsClient _settingClient;
        public string LabelTextSuccess { get; set; }
        [BindProperty]
        public int StorageDuration { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string WelcomeMessage { get; set; }

        public async Task<ActionResult> OnGetAsync(string handler)
        {
            _settingClient = new SettingsClient();

            StorageDuration = await _settingClient.GetStorageDurationAsync();
            WelcomeMessage = await _settingClient.GetWelcomeTextAsync();
            Password = await _settingClient.GetPasswordAsync();

            if (handler == null)
            {
                return RedirectToPage("/LoginPage", "Settings");
            }

            return Page();
        }

        public async Task<ActionResult> OnPostAsync(string handler)
        {
            _settingClient = new SettingsClient();

            try
            {
                await _settingClient.PostPasswordAsync(Password);
                await _settingClient.PostStorageDurationAsync(StorageDuration);
                await _settingClient.PostWelcomeTextAsync(WelcomeMessage);
            }
            catch(ApiException<ProblemDetails> ex)
            {
                ModelState.AddModelError("ApiException", ex.Result.Detail);
            }

            LabelTextSuccess = "Einstellungen erfolgreich gespeichert!";

            return Page();
        }
    }
}
