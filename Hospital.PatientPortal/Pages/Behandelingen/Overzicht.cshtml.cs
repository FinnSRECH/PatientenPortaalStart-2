using System.Security.Claims;
using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hospital.PatientPortal.Pages.Behandelingen;

[Authorize]
public class OverzichtModel : PageModel
{
	private readonly HospitalDataService _data;

	public OverzichtModel(HospitalDataService data)
	{
		_data = data;
	}

	public IReadOnlyList<Treatment> Treatments { get; private set; }
		= Array.Empty<Treatment>();

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

		Treatments = _data.GetTreatments(patientId);

		var patientName =
			$"{patient.FirstName} {patient.LastName}";

		_data.StartAuditLog(
			userId: patient.Id,
			userName: patientName,
			patientId: patient.Id,
			patientName: patientName,
			action: "Raadplegen",
			resource: "Behandelingen");

		return Page();
	}
}