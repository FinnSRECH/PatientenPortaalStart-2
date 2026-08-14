using System.Security.Claims;
using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Hospital.PatientPortal.Pages.Behandelingen;
public class OverzichtModel(HospitalDataService data) : PageModel
{
    public IReadOnlyList<Treatment> Treatments { get; private set; } = Array.Empty<Treatment>();
    public void OnGet() => Treatments = data.GetTreatments(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
}
