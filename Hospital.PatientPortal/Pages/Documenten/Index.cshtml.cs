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
	private readonly IWebHostEnvironment _environment;

	public IndexModel(
		HospitalDataService data,
		IWebHostEnvironment environment)
	{
		_data = data;
		_environment = environment;
	}

	public IReadOnlyList<PatientDocument> Documents { get; private set; }
		= new List<PatientDocument>();

	public IActionResult OnGet()
	{
		var patientIdValue =
			User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (!int.TryParse(patientIdValue, out var patientId))
		{
			return RedirectToPage("/Account/Inloggen");
		}

		var patient = _data.GetPatient(patientId);

		if (patient is null)
		{
			return RedirectToPage("/Account/Inloggen");
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

	public IActionResult OnGetDownload(int id)
	{
		var patientIdValue =
			User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (!int.TryParse(patientIdValue, out var patientId))
		{
			return Unauthorized();
		}

		var patient = _data.GetPatient(patientId);

		if (patient is null)
		{
			return Unauthorized();
		}

		var document = _data
			.GetDocuments(patientId)
			.FirstOrDefault(d => d.Id == id);

		if (document is null)
		{
			return NotFound();
		}

		if (string.IsNullOrWhiteSpace(document.FilePath))
		{
			return NotFound();
		}

		var physicalPath = Path.Combine(
			_environment.ContentRootPath,
			document.FilePath);

		if (!System.IO.File.Exists(physicalPath))
		{
			return NotFound();
		}

		var patientName =
			$"{patient.FirstName} {patient.LastName}";

		_data.StartAuditLog(
			userId: patient.Id,
			userName: patientName,
			patientId: patient.Id,
			patientName: patientName,
			action: "Raadplegen",
			resource: $"Document: {document.FileName}");

		return PhysicalFile(
			physicalPath,
			document.ContentType,
			document.FileName);
	}
}