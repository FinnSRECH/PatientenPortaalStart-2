using System.Security.Claims;
using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Hospital.PatientPortal.Pages.Consultaties;
public class HistoriekModel(HospitalDataService data) : PageModel
{
    public IReadOnlyList<Consultation> Consultations { get; private set; } = Array.Empty<Consultation>();
    public void OnGet() => Consultations = data.GetConsultationHistory(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
}
