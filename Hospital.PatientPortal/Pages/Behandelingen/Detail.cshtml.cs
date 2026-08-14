using System.Security.Claims;
using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hospital.PatientPortal.Pages.Behandelingen;

[Authorize]
public class DetailModel : PageModel
{
	private readonly HospitalDataService _data;

	public DetailModel(HospitalDataService data)
	{
		_data = data;
	}

	public Treatment Treatment { get; private set; } = null!;

	public IReadOnlyList<Consultation> Consultations { get; private set; }
		= Array.Empty<Consultation>();

	public IReadOnlyList<Operation> Operations { get; private set; }
		= Array.Empty<Operation>();

	public IActionResult OnGet(int id)
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

		var treatment =
			_data.GetTreatment(id, patientId);

		if (treatment is null)
		{
			return NotFound();
		}

		Treatment = treatment;

		Consultations =
			_data.GetConsultationsForTreatment(id, patientId);

		Operations =
			_data.GetOperationsForTreatment(id, patientId);

		var patientName =
			$"{patient.FirstName} {patient.LastName}";

		_data.StartAuditLog(
			userId: patient.Id,
			userName: patientName,
			patientId: patient.Id,
			patientName: patientName,
			action: "Raadplegen",
			resource: $"Behandeling: {treatment.Name}");

		return Page();
	}
}