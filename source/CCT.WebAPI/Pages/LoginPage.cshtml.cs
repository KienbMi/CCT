using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCT.APIService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CCT.WebAPI.Pages
{
	public class LoginPageModel : PageModel
	{
		private SettingsClient _settingClient;
		private string _password;

		[BindProperty]
		public string LastPage { get; set; }

		[BindProperty]
		public string LabelTextWrongPassword { get; set; }

		[BindProperty]
		public string Password { get; set; }
		public async Task<ActionResult> OnGetAsync(string handler)
		{
			LastPage = handler;
			// ------- request cookie ---------------------
			_settingClient = new SettingsClient();
			var cookieValue = Request.Cookies["MyCookieId"];
			string _pass;
			try
			{
				_pass = await _settingClient.GetPasswordAsync();
			}
			catch (Exception)
			{
				return RedirectToPage("/LoginPage", "Index");
			}

			// --------------------------------------------
			if (handler == null || cookieValue == null || cookieValue != _pass)
			{
				return Page();
			}
			if(cookieValue == _pass)
			{
				if (LastPage == "Index")
				{
					return RedirectToPage("/RegistrationOverview", handler);
				}
				else if (LastPage == "Settings")
				{
					return RedirectToPage("/SettingsPage", handler);
				}
			}

			return Page();
		}

		public async Task<ActionResult> OnPostAsync(string handler)
		{
			_settingClient = new SettingsClient();

			_password = await _settingClient.GetPasswordAsync();
			
			// ------------- set cookie ----------------------
			CookieOptions option = new CookieOptions
			{
				Path = "/",
				HttpOnly = false,
				IsEssential = true,
				Expires = DateTime.Now.AddMinutes(15)
			};
			Response.Cookies.Append("MyCookieId", $"{_password}", option);
			// ---- using Microsoft.AspNetCore.Http ----------

			if (Password == _password)
			{
				if (LastPage == "Index")
				{
					return RedirectToPage("/RegistrationOverview", handler);
				}
				else if (LastPage == "Settings")
				{
					return RedirectToPage("/SettingsPage", handler);
				}
			}

			LabelTextWrongPassword = "Passwort ist falsch!";

			return Page();
		}
	}
}
