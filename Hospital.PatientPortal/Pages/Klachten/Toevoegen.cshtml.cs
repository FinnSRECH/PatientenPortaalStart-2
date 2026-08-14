using System.Security.Claims;
using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hospital.PatientPortal.Pages.Klachten;

public class ToevoegenModel(HospitalDataService data) : PageModel
{
    [BindProperty] public Complaint Complaint { get; set; } = new();

    public IActionResult OnPost()
    {
        ModelState.Remove("Complaint.PatientId");
        if (!ModelState.IsValid) return Page();
        Complaint.PatientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        data.AddComplaint(Complaint);
        TempData["Success"] = "Uw klacht is geregistreerd.";
        return RedirectToPage("/Klachten/Overzicht");
    }
}
