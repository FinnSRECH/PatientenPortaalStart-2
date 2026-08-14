using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hospital.PatientPortal.Pages.Documenten;

[Authorize]
public class UploadenModel : PageModel
{
	private readonly HospitalDataService _data;
	private readonly IWebHostEnvironment _environment;

	private const long MaximumFileSize = 5 * 1024 * 1024;

	private static readonly string[] AllowedExtensions =
	{
		".pdf",
		".jpg",
		".jpeg",
		".png"
	};

	public UploadenModel(
		HospitalDataService data,
		IWebHostEnvironment environment)
	{
		_data = data;
		_environment = environment;
	}

	[BindProperty]
	[Required(ErrorMessage = "Selecteer een document om te uploaden.")]
	public IFormFile? UploadFile { get; set; }

	[BindProperty]
	[Required(ErrorMessage = "Vul een omschrijving in.")]
	[StringLength(
		250,
		ErrorMessage = "De omschrijving mag maximaal 250 tekens bevatten.")]
	public string Description { get; set; } = string.Empty;

	public void OnGet()
	{
	}

	public async Task<IActionResult> OnPostAsync()
	{
		var patientIdValue =
			User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (!int.TryParse(patientIdValue, out var patientId))
		{
			return RedirectToPage("/Account/Login");
		}

		if (UploadFile is null)
		{
			ModelState.AddModelError(
				nameof(UploadFile),
				"Selecteer een document om te uploaden.");

			return Page();
		}

		if (UploadFile.Length == 0)
		{
			ModelState.AddModelError(
				nameof(UploadFile),
				"Het geselecteerde bestand is leeg.");

			return Page();
		}

		if (UploadFile.Length > MaximumFileSize)
		{
			ModelState.AddModelError(
				nameof(UploadFile),
				"Het bestand is groter dan 5 MB.");

			return Page();
		}

		var extension =
			Path.GetExtension(UploadFile.FileName).ToLowerInvariant();

		if (!AllowedExtensions.Contains(extension))
		{
			ModelState.AddModelError(
				nameof(UploadFile),
				"Alleen PDF-, JPG-, JPEG- en PNG-bestanden zijn toegestaan.");

			return Page();
		}

		if (!ModelState.IsValid)
		{
			return Page();
		}

		var patient = _data.GetPatient(patientId);

		if (patient is null)
		{
			return RedirectToPage("/Account/Login");
		}

		var uploadDirectory = Path.Combine(
			_environment.WebRootPath,
			"uploads",
			patientId.ToString());

		Directory.CreateDirectory(uploadDirectory);

		// Gebruik niet rechtstreeks de originele bestandsnaam op de server.
		// Een willekeurige naam voorkomt conflicten en ongewenste paden.
		var safeFileName = $"{Guid.NewGuid()}{extension}";

		var physicalFilePath =
			Path.Combine(uploadDirectory, safeFileName);

		await using (var stream =
			new FileStream(physicalFilePath, FileMode.Create))
		{
			await UploadFile.CopyToAsync(stream);
		}

		var document = new PatientDocument
		{
			PatientId = patientId,
			FileName = Path.GetFileName(UploadFile.FileName),
			Description = Description.Trim(),
			ContentType = UploadFile.ContentType,
			FilePath = $"/uploads/{patientId}/{safeFileName}",
			UploadedBy = $"{patient.FirstName} {patient.LastName}",
			UploadedByHealthcareProvider = false
		};

		_data.AddDocument(document);

		TempData["SuccessMessage"] =
			"Uw document is succesvol toegevoegd aan uw dossier.";

		return RedirectToPage("/Documenten/Index");
	}
}