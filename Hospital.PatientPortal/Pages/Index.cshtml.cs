using System.Security.Claims;
using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hospital.PatientPortal.Pages;

public class IndexModel(HospitalDataService data) : PageModel
{
    public Patient Patient { get; private set; } = null!;
    public Treatment? ActiveTreatment { get; private set; }
    public Consultation? NextConsultation { get; private set; }

    public void OnGet()
    {
        var patientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Patient = data.GetPatient(patientId)!;
        ActiveTreatment = data.GetActiveTreatment(patientId);
        NextConsultation = data.GetConsultations(patientId).FirstOrDefault(c => c.StartTime >= DateTime.Now);
    }
}
