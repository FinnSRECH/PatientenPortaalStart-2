using System.Security.Claims;
using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Hospital.PatientPortal.Pages.Evaluaties;
public class OverzichtModel(HospitalDataService data) : PageModel
{
    public IReadOnlyList<Evaluation> Evaluations { get; private set; } = Array.Empty<Evaluation>();
    public void OnGet() => Evaluations = data.GetEvaluations(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!));
}
