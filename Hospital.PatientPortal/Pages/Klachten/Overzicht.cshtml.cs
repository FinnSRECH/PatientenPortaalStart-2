using System.Security.Claims;
using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Hospital.PatientPortal.Pages.Klachten;

public class OverzichtModel(HospitalDataService data) : PageModel
{
    public IReadOnlyList<Complaint> Complaints { get; private set; } = Array.Empty<Complaint>();
    public void OnGet()
    {
        var patientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Complaints = data.GetComplaints(patientId);
    }
}
