using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCT.APIService;
using CCT.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [BindProperty]
        public string SelectedType { get; set; }
        [BindProperty]
        public List<SelectListItem> NfcReaderTypes { get; set; }

        public async Task<ActionResult> OnGetAsync(string handler)
        {
            _settingClient = new SettingsClient();

            StorageDuration = await _settingClient.GetStorageDurationAsync();
            WelcomeMessage = await _settingClient.GetWelcomeTextAsync();
            Password = await _settingClient.GetPasswordAsync();
            var type = await _settingClient.GetNfcReaderTypesAsync();
            SelectedType = type.ToString();

            NfcReaderTypes = new List<SelectListItem>();

            NfcReaderTypes.Add(new SelectListItem
            {
                Value = Core.NfcReaderType.RC522.ToString(),
                Text = Core.NfcReaderType.RC522.ToString()
            });

            NfcReaderTypes.Add(new SelectListItem
            {
                Value = Core.NfcReaderType.uFr.ToString(),
                Text = Core.NfcReaderType.uFr.ToString()
            });


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
                await _settingClient.PostNfcReaderTypeAsync((APIService.NfcReaderType)Enum.Parse(typeof(APIService.NfcReaderType), SelectedType));
            }
            catch(ApiException<ProblemDetails> ex)
            {
                ModelState.AddModelError("ApiException", ex.Result.Detail);
            }

            NfcReaderTypes = new List<SelectListItem>();

            NfcReaderTypes.Add(new SelectListItem
            {
                Value = Core.NfcReaderType.RC522.ToString(),
                Text = Core.NfcReaderType.RC522.ToString()
            });

            NfcReaderTypes.Add(new SelectListItem
            {
                Value = Core.NfcReaderType.uFr.ToString(),
                Text = Core.NfcReaderType.uFr.ToString()
            });

            var type = await _settingClient.GetNfcReaderTypesAsync();
            SelectedType = type.ToString();

            LabelTextSuccess = "Einstellungen erfolgreich gespeichert!";

            return Page();
        }
    }
}
