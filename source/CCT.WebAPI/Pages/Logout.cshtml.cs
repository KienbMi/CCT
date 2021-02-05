using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCT.APIService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CCT.WebAPI.Pages
{
	public class LogoutModel : PageModel
	{
		public async Task<IActionResult> OnGetAsync()
		{
			// ------- request cookie ---------------------
			var cookieValue = Request.Cookies["MyCookieId"];
			// --------------------------------------------
			if (String.IsNullOrEmpty(cookieValue))
			{
				return RedirectToPage("Index");
			}
			else
			{
				// ------------ delete cookie ------------------
				foreach (var cookie in HttpContext.Request.Cookies)
				{
					Response.Cookies.Delete(cookie.Key);
				}
				// --------------------------------------------
			}

			return RedirectToPage("Index");
		}
	}
}
