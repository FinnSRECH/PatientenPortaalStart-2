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
		var patientIdValue =
			User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (!int.TryParse(patientIdValue, out var patientId))
		{
			return RedirectToPage("/Account/Login");
		}

		var patient = _data.GetPatient(patientId);

		if (patient is null)
		{
			return RedirectToPage("/Account/Login");
		}

		Documents = _data.GetDocuments(patientId);

		var patientName =
			$"{patient.FirstName} {patient.LastName}";

		_data.StartAuditLog(
			userId: patient.Id,
			userName: patientName,
			patientId: patient.Id,
			patientName: patientName,
			action: "Raadplegen",
			resource: "Documenten");

		return Page();
	}
}