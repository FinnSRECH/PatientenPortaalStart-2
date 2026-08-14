using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Hospital.PatientPortal.Pages.Documenten;

[Authorize]
public class IndexModel : PageModel
{
	private readonly HospitalDataService _data;

	public IndexModel(HospitalDataService data)
	{
		_data = data;
	}

	public IReadOnlyList<PatientDocument> Documents { get; private set; }
		= new List<PatientDocument>();

	public IActionResult OnGet()
	{
		var patientIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (!int.TryParse(patientIdValue, out var patientId))
		{
			return RedirectToPage("/Account/Login");
		}

		Documents = _data.GetDocuments(patientId);

		return Page();
	}
}