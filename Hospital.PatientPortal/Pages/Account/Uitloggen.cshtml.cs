using System.Security.Claims;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hospital.PatientPortal.Pages.Account;

public class UitloggenModel : PageModel
{
	private readonly HospitalDataService _data;

	public UitloggenModel(HospitalDataService data)
	{
		_data = data;
	}

	public async Task<IActionResult> OnGetAsync()
	{
		// Sluit een eventueel nog actieve dossierraadpleging af
		// voordat de gebruiker wordt uitgelogd.
		var userIdValue =
			User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (int.TryParse(userIdValue, out var userId))
		{
			_data.CloseActiveAuditLogs(userId);
		}

		await HttpContext.SignOutAsync(
			CookieAuthenticationDefaults.AuthenticationScheme);

		return RedirectToPage("/Account/Inloggen");
	}
}
