using System.Security.Claims;
using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Hospital.PatientPortal.Pages.Behandelingen;
public class DetailModel(HospitalDataService data) : PageModel
{
    public Treatment Treatment { get; private set; } = null!;
    public IReadOnlyList<Consultation> Consultations { get; private set; } = Array.Empty<Consultation>();
    public IActionResult OnGet(int id)
    {
        var patientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var treatment = data.GetTreatment(id, patientId);
        if (treatment is null) return NotFound();
        Treatment = treatment; Consultations = data.GetConsultationsForTreatment(id, patientId); return Page();
    }
}
