using System.Security.Claims;
using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hospital.PatientPortal.Pages.Klachten;

[Authorize]
public class ToevoegenModel : PageModel
{
	private readonly HospitalDataService _data;

	public ToevoegenModel(HospitalDataService data)
	{
		_data = data;
	}

	[BindProperty]
	public Complaint Complaint { get; set; } = new();

	public IActionResult OnPost()
	{
		ModelState.Remove("Complaint.PatientId");

		if (!ModelState.IsValid)
		{
			return Page();
		}

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

		Complaint.PatientId = patientId;

		_data.AddComplaint(Complaint);

		var patientName =
			$"{patient.FirstName} {patient.LastName}";

		_data.StartAuditLog(
			userId: patient.Id,
			userName: patientName,
			patientId: patient.Id,
			patientName: patientName,
			action: "Toevoegen",
			resource: "Klacht");

		TempData["Success"] =
			"Uw klacht is geregistreerd.";

		return RedirectToPage("/Klachten/Overzicht");
	}
}