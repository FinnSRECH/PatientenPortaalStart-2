using System.Security.Claims;
using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Hospital.PatientPortal.Pages.Planning;
public class OverzichtModel(HospitalDataService data) : PageModel
{
    public IReadOnlyList<Consultation> Consultations { get; private set; } = Array.Empty<Consultation>();
    public void OnGet() => Consultations = data.GetConsultations(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
}
