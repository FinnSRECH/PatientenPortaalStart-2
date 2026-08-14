using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Hospital.PatientPortal.Pages.Profiel;

public class OverzichtModel(HospitalDataService data) : PageModel
{
    [BindProperty] public ProfileInput Input { get; set; } = new();
    public string Email { get; private set; } = string.Empty;

    public class ProfileInput
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Phone] public string PhoneNumber { get; set; } = string.Empty;
        [Required] public string Address { get; set; } = string.Empty;
    }

    public void OnGet()
    {
        var p = data.GetPatient(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!))!;
        Email = p.Email;
        Input = new ProfileInput { FirstName = p.FirstName, LastName = p.LastName, PhoneNumber = p.PhoneNumber, Address = p.Address };
    }

    public IActionResult OnPost()
    {
        var p = data.GetPatient(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!))!;
        Email = p.Email;
        if (!ModelState.IsValid) return Page();
        p.FirstName = Input.FirstName; p.LastName = Input.LastName; p.PhoneNumber = Input.PhoneNumber; p.Address = Input.Address;
        data.UpdatePatient(p);
        TempData["Success"] = "Uw contactgegevens zijn bijgewerkt.";
        return RedirectToPage();
    }
}
