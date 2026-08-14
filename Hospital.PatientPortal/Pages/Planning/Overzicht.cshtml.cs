using System.Security.Claims;
using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hospital.PatientPortal.Pages.Planning;

[Authorize]
public class OverzichtModel : PageModel
{
	private readonly HospitalDataService _data;

	public OverzichtModel(HospitalDataService data)
	{
		_data = data;
	}

	public IReadOnlyList<Consultation> Consultations { get; private set; }
		= Array.Empty<Consultation>();

	public IReadOnlyList<Operation> Operations { get; private set; }
		= Array.Empty<Operation>();

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

		Consultations = _data.GetConsultations(patientId);
		Operations = _data.GetOperations(patientId);

		var patientName =
			$"{patient.FirstName} {patient.LastName}";

		_data.StartAuditLog(
			userId: patient.Id,
			userName: patientName,
			patientId: patient.Id,
			patientName: patientName,
			action: "Raadplegen",
			resource: "Planning");

		return Page();
	}
}