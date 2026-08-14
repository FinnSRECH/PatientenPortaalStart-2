using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hospital.PatientPortal.Pages.Profiel;

[Authorize]
public class OverzichtModel : PageModel
{
	private readonly HospitalDataService _data;

	public OverzichtModel(HospitalDataService data)
	{
		_data = data;
	}

	[BindProperty]
	public ProfileInput Input { get; set; } = new();

	public string Email { get; private set; } = string.Empty;

	public class ProfileInput
	{
		[Required(ErrorMessage = "Voornaam is verplicht.")]
		public string FirstName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Achternaam is verplicht.")]
		public string LastName { get; set; } = string.Empty;

		[Phone(ErrorMessage = "Vul een geldig telefoonnummer in.")]
		public string PhoneNumber { get; set; } = string.Empty;

		[Required(ErrorMessage = "Adres is verplicht.")]
		public string Address { get; set; } = string.Empty;
	}

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

		Email = patient.Email;

		Input = new ProfileInput
		{
			FirstName = patient.FirstName,
			LastName = patient.LastName,
			PhoneNumber = patient.PhoneNumber,
			Address = patient.Address
		};

		return Page();
	}

	public IActionResult OnPost()
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

		Email = patient.Email;

		if (!ModelState.IsValid)
		{
			return Page();
		}

		patient.FirstName = Input.FirstName;
		patient.LastName = Input.LastName;
		patient.PhoneNumber = Input.PhoneNumber;
		patient.Address = Input.Address;

		_data.UpdatePatient(patient);

		var patientName =
			$"{patient.FirstName} {patient.LastName}";

		_data.StartAuditLog(
			userId: patient.Id,
			userName: patientName,
			patientId: patient.Id,
			patientName: patientName,
			action: "Wijzigen",
			resource: "Profiel/contactgegevens");

		TempData["Success"] =
			"Uw contactgegevens zijn bijgewerkt.";

		return RedirectToPage();
	}
}